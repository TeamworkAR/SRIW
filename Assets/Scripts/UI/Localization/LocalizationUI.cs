using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // List to keep track of the original locale indices after sorting
        private List<int> m_LocaleIndexMapping = new List<int>();

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
            // Use the mapping to get the correct locale index
            int localeIndex = m_LocaleIndexMapping[index];
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];

            ScormManager.Instance.StoreCustomData(Consts.ScormKeys.k_LOCALIZATION_SERIALIZATION_KEY, localeIndex.ToString());
        }

        IEnumerator COR_LOAD()
        {
            yield return LocalizationSettings.InitializationOperation;

            var options = new List<(TMP_Dropdown.OptionData option, int index)>();

            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add((new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.NativeName), i));
            }

            // Sort the options alphabetically by locale name
            options = options.OrderBy(o => o.option.text).ToList();

            // Clear the index mapping and populate it based on the sorted options
            m_LocaleIndexMapping.Clear();
            m_LocaleIndexMapping.AddRange(options.Select(o => o.index));

            // Update the dropdown with sorted options
            m_Dropdown.options = options.Select(o => o.option).ToList();

            // Update the selected value to match the sorted list
            m_Dropdown.value = m_LocaleIndexMapping.IndexOf(selected);

            m_Dropdown.onValueChanged.AddListener(LocaleSelected);
        }
    }
}
