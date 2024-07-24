using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI.Localization
{
    public class LocalizationUI : BaseUICanvasGroup
    {
        [SerializeField] private TMP_Dropdown m_Dropdown = null;

        private Coroutine m_Runningg = null;

        private Coroutine m_LocaleLoadingCoroutine = null;

        public override bool IsDone()
        {
            return base.IsDone() && IsOnScreen() == false && m_Runningg == null && m_LocaleLoadingCoroutine == null;
        }

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            m_Runningg = StartCoroutine(COR_LOAD());
        }

        protected override void OnShowCompleted()
        {
            m_Runningg = null;
            
            base.OnShowCompleted();
        }

        protected override void OnHideStart()
        {
            m_LocaleLoadingCoroutine = StartCoroutine(COR_LoadLocale());

            IEnumerator COR_LoadLocale() 
            {
                yield return LocalizationSettings.SelectedLocaleAsync;

                m_LocaleLoadingCoroutine = null;
            }
        }

        protected override void OnHideCompleted()
        {
            m_Dropdown.onValueChanged.RemoveListener(LocaleSelected);
            
            base.OnHideCompleted();
        }

        private void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

            ScormManager.Instance.StoreCustomData(Consts.ScormKeys.k_LOCALIZATION_SERIALIZATION_KEY, index.ToString());
        }

        IEnumerator COR_LOAD()
        {
            // This is also called in GameManager, we should get rid of this line and make this coroutine a normal method.
            yield return LocalizationSettings.InitializationOperation;
            
            var options = new List<TMP_Dropdown.OptionData>();
            
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.NativeName));
            }
            m_Dropdown.options = options;

            m_Dropdown.value = selected;
            m_Dropdown.onValueChanged.AddListener(LocaleSelected);
        }
    }
}