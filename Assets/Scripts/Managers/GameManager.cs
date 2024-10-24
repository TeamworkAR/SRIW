using System.Collections;
using Core;
using Data;
using Data.ScenarioSettings;
using NodeEditor.Graphs;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Managers
{
    // TODO: Here flow graph should be started
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] public Graph m_FlowGraph = null;
        
        [SerializeField] public ScenarioSettings m_ScenarioSettings = null;

        [SerializeField] private DevSettings m_DevSettings = null;

        private Graph.GraphInstance m_GraphInstance = null;
        
        public ScenarioSettings ScenarioSettings => m_ScenarioSettings;

        public DevSettings DevSettings => m_DevSettings;

        public Graph.GraphInstance GraphInstance => m_GraphInstance;

        private IEnumerator Start()
        {
            // Wait for localization to initialize
            // This is useful (and mandatory) when restoring a node, else our localized UIs might broke.
            yield return LocalizationSettings.InitializationOperation;

            // Try restore previously selected locale
            var suspendedData = ScormManager.Instance.GetCustomString(Consts.ScormKeys.k_LOCALIZATION_SERIALIZATION_KEY);
            if (string.IsNullOrEmpty(suspendedData) || LocalizationSettings.AvailableLocales.Locales.Count <= int.Parse(suspendedData))
            {

            }
            else 
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(suspendedData)];

                yield return LocalizationSettings.SelectedLocaleAsync;
            }

            // Application.targetFrameRate = 60;

            m_GraphInstance = m_FlowGraph.GetInstance(this);

            yield return null;
            
            m_GraphInstance.Run();
        }

        public void Back()
        {
            m_GraphInstance.Back();
        }

        public void Reset()
        {
            m_GraphInstance.Interrupt();
            m_GraphInstance.Run();
        }
    }
}