using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Managers
{
    /// <summary>
    /// This is a short-term <b>interim</b> solution to get around the need to run async operations to get LocalizedStrings on WebGL.
    /// Long term, the work should be done to refactor toward async ops rather than relying on this solution. See remarks for rationale.
    /// </summary>
    /// <remarks> This solution does not account for or directly circumvents much of the functionality of the localization system, but addresses the immediate need
    /// to work with the preloaded localized strings without having to handle asynchronous operations at runtime in various fields.
    /// </remarks>
    public class LocalizationManager: SingletonBehaviour<LocalizationManager>
    {
        [SerializeField] private List<TableReference> _stringTableRefs = new();
        private Dictionary<string, Dictionary<long,string>> _localizedValues = new();
        private Dictionary<string, Dictionary<string,string>> _localizedValuesByEntryName = new();
        private bool _initialized;
        public bool Initialized => _initialized;
        private int _stringTablesLoaded = 0;
        public bool StringTablesLoaded => _stringTablesLoaded == _stringTableRefs.Count;

        /// <summary>
        /// Triggers when initialized, or locale changes
        /// </summary>
        public UnityEvent OnLocalizationChange;
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            LocalizationSettings.SelectedLocaleChanged += HandleLocaleChanged;
            if (!LocalizationSettings.InitializationOperation.IsDone)
                LocalizationSettings.InitializationOperation.Completed += _ => Initialize();
            else
                Initialize();
        }
        
        private void Initialize()
        {
            if(_initialized)
                return;
            _initialized = true;
            CacheAllLocalizedStringValues();
            OnLocalizationChange?.Invoke();
        }

        public void InvalidateLocale()
        {
            _stringTablesLoaded = 0;
        }
        
        private void HandleLocaleChanged(Locale locale)
        {
            _initialized = true;
            Debug.Log("Localization Manager:" + locale);
            CacheAllLocalizedStringValues();
            OnLocalizationChange?.Invoke();
        }
        
        private void CacheAllLocalizedStringValues()
        {
            _stringTablesLoaded = 0;
            _localizedValues.Clear();
            _localizedValuesByEntryName.Clear();

            foreach (var stringTableRef in _stringTableRefs)
            {
                LocalizationSettings.StringDatabase.GetTableAsync(stringTableRef, LocalizationSettings.SelectedLocale).Completed += handle =>
                {
                    /// <summary>
                    /// Force load of SharedTable and avoid release as workaround fix of WebGL NullReference issue
                    /// </summary>
                    var operation = Addressables.LoadAssetAsync<SharedTableData>(handle.Result.SharedData.TableCollectionNameGuid.ToString("N"));
                    Addressables.ResourceManager.Acquire(operation);
                
                    var values = new Dictionary<long, string>();
                    var entryValues = new Dictionary<string, string>();
                    foreach (var value in handle.Result.Values)
                    {
                        if(value.IsSmart) Debug.LogWarning($"Potential localization error, please verify results for key: {value.Key} id: {value.KeyId}");
                        //NOTE, When dealing with instances of StringTableEntry, GetLocalizedString() does not use WaitForCompletion, making it safe for use within WebGL
                        values.Add(value.KeyId, value.GetLocalizedString());
                        entryValues.Add(value.Key, value.GetLocalizedString());
                    }
                    _localizedValues.Add(stringTableRef, values);
                    _localizedValuesByEntryName.Add(stringTableRef, entryValues);
                    _stringTablesLoaded++;
                };
            }
        }
        
        public string GetLocalizedValue(LocalizedString localizedString)
        {
            var rtn = "";
            if (!_initialized) return rtn;
            
            if(string.IsNullOrEmpty(localizedString.TableReference.TableCollectionName))
            {
                Debug.LogWarning("Null/Empty TableReference/TableCollectionName");
                return rtn;
            }

            if (_localizedValues.TryGetValue(localizedString.TableReference.TableCollectionName,
                    out var values))
            {
                values.TryGetValue(localizedString.TableEntryReference.KeyId, out rtn);
            }
            
            return rtn;
        }
        public string GetLocalizedValue(string tableCollectionName, string localizationKey)
        {
            var rtn = "";
            if (!_initialized) return rtn;
            
            if(string.IsNullOrEmpty(tableCollectionName))
            {
                Debug.LogWarning("Null/Empty TableReference/TableCollectionName");
                return rtn;
            }

            if (_localizedValuesByEntryName.TryGetValue(tableCollectionName,
                    out var values))
            {
                values.TryGetValue(localizationKey, out rtn);
            }
            
            return rtn;
        }
    }
}