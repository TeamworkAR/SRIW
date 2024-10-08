using System;
using Core;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI.Localization
{
    public class LocalizationUI : BaseUICanvasGroup
    {
        [SerializeField] private CanvasGroup m_ButtonCanvasGroup = null;
        [SerializeField] private TMP_Dropdown m_Dropdown = null;

        [SerializeField] private Button m_StartButton = null;


        [SerializeField] private RawImage m_loadingImage = null;

        private Coroutine m_Runningg = null;

        private Coroutine m_LocaleLoadingCoroutine = null;

        // need to hard-code as we can't sort by native names
        public List<Tuple<string, string>> LanguageNamesSorted => new List<Tuple<string, string>> {
            new Tuple<string, string>("English", "en"),
            new Tuple<string, string>("Čeština", "cs"),
            new Tuple<string, string>("Deutsch", "de"),
            new Tuple<string, string>("Español (España)", "es-ES"),
            new Tuple<string, string>("Español (Estados Unidos)", "es-US"),
            new Tuple<string, string>("Français (Canada)", "fr-CA"),
            new Tuple<string, string>("Français (France)", "fr-FR"),
            new Tuple<string, string>("Italiano", "it"),
            new Tuple<string, string>("Nederlands", "nl-NL"),
            new Tuple<string, string>("Polski", "pl-PL"),
            new Tuple<string, string>("Português", "pt-PT"),
            new Tuple<string, string>("Slovenčina", "sk-SK"),
            new Tuple<string, string>("Українська", "uk-UA")
        };

        public override bool IsDone()
        {
            return base.IsDone() && IsOnScreen() == false && m_Runningg == null && m_LocaleLoadingCoroutine == null;
        }

        protected override void OnShowStart()
        {
            base.OnShowStart();
            m_ButtonCanvasGroup.interactable = true;
            m_ButtonCanvasGroup.alpha = 1f;
            m_loadingImage.color = new Color(1f, 1f, 1f, 0f);

            m_Runningg = StartCoroutine(COR_LOAD());
        }

        protected override void OnShowCompleted()
        {
            m_Runningg = null;

            base.OnShowCompleted();
        }

        public void EndScene()
        {
            m_ButtonCanvasGroup.interactable = false;
            m_LocaleLoadingCoroutine = StartCoroutine(WaitForLocaleLoaded());
        }
        IEnumerator WaitForLocaleLoaded()
        {
            yield return Helpers.UI.COR_Fade(m_ButtonCanvasGroup, 1f, 0f,
                GameManager.Instance.DevSettings.BaseFadeDuration);


            float target = 1f;
            float current = 0f;
            while (!LocalizationManager.Instance.IsLoaded)
            {
                if (current >= target && target > 0)
                {
                    target = 0f;
                }
                else if (current <= target && target == 0)
                {
                    target = 1f;
                }
                current = Mathf.MoveTowards(current, target, Time.deltaTime);
                m_loadingImage.color = new Color(1f, 1f, 1f, current);
                yield return null;
            }
            m_LocaleLoadingCoroutine = null;
            Hide();
        }

        protected override void OnHideCompleted()
        {
            m_Dropdown.onValueChanged.RemoveListener(LocaleSelected);
            base.OnHideCompleted();
        }

        private void LocaleSelected(int index)
        {
            int selected = index;
            var selectedLocale = LanguageNamesSorted[index];
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var localNativeName = LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code;
                Debug.Log($"LOOKING FOR: {localNativeName}");
                Debug.Log($"CURRENT: {selectedLocale.Item2}");
                if (localNativeName.Equals(selectedLocale.Item2, StringComparison.OrdinalIgnoreCase))
                {
                    selected = i;
                    Debug.Log($"FOUND: {localNativeName}");
                    break;
                }
            }

            LocalizationManager.Instance.InvalidateLocale();
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selected];
            LocalizationManager.Instance.SetAudioLocale(LocalizationSettings.AvailableLocales.Locales[selected]);

            ScormManager.Instance.StoreCustomData(Consts.ScormKeys.k_LOCALIZATION_SERIALIZATION_KEY, selected.ToString());
        }

        IEnumerator COR_LOAD()
        {
            // This is also called in GameManager, we should get rid of this line and make this coroutine a normal method.
            yield return LocalizationSettings.InitializationOperation;
            PopulateTextLocaleDropdown();
        }

        private void PopulateTextLocaleDropdown()
        {
            var options = new List<TMP_Dropdown.OptionData>();

            int selected = 0;
            var languageNamesSorted = LanguageNamesSorted;
            for (int i = 0; i < languageNamesSorted.Count; ++i)
            {
                var optionToAdd = new TMP_Dropdown.OptionData(languageNamesSorted[i].Item1);
                options.Add(optionToAdd);
                var localNativeName = LocalizationSettings.SelectedLocale.Identifier.Code;
                if (localNativeName.Equals(languageNamesSorted[i].Item2, StringComparison.OrdinalIgnoreCase))
                {
                    selected = i;
                }
            }

            m_Dropdown.options = options;

            m_Dropdown.value = selected;
            m_Dropdown.onValueChanged.AddListener(LocaleSelected);
        }
    }
}