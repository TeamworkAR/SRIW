using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{

    public class LocalizationManager : SingletonBehaviour<LocalizationManager>
    {
        [SerializeField] private List<TableReference> _stringTableRefs = new();
        private Dictionary<string, Dictionary<long, string>> _localizedValues = new();
        private Dictionary<string, Dictionary<string, string>> _localizedValuesByEntryName = new();
        private bool _initialized;
        public bool Initialized => _initialized;
        private int _numLoadedStringTables = 0;
        public bool IsLoaded => _numLoadedStringTables == _stringTableRefs.Count;

        public Locale AudioLocale { get; private set; }

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
        public void SetAudioLocale(Locale locale)
        {
            AudioLocale = locale;
        }

        private void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;
            CacheAllLocalizedStringValues();
            OnLocalizationChange?.Invoke();
        }

        public void InvalidateLocale()
        {
            _numLoadedStringTables = 0;
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
            _numLoadedStringTables = 0;
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
                        if (value.IsSmart) Debug.LogWarning($"Potential localization error, please verify results for key: {value.Key} id: {value.KeyId}");
                        //NOTE, When dealing with instances of StringTableEntry, GetLocalizedString() does not use WaitForCompletion, making it safe for use within WebGL
                        values.Add(value.KeyId, value.GetLocalizedString());
                        entryValues.Add(value.Key, value.GetLocalizedString());
                    }
                    _localizedValues.Add(stringTableRef, values);
                    _localizedValuesByEntryName.Add(stringTableRef, entryValues);
                    _numLoadedStringTables++;
                };
            }
        }

        public string GetLocalizedValue(LocalizedString localizedString)
        {
            var rtn = "";
            if (!_initialized) return rtn;

            if (string.IsNullOrEmpty(localizedString.TableReference.TableCollectionName))
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

            if (string.IsNullOrEmpty(tableCollectionName))
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

        public IEnumerator COR_LoadAudioClip(LocalizedAsset<AudioClip> localizedClip, Action<AudioClip> onLoaded)
        {
            localizedClip.LocaleOverride = AudioLocale;
            var handle = localizedClip.LoadAssetAsync();
            while (!handle.IsDone)
                yield return null;

            onLoaded?.Invoke(handle.Result);
        }

        // async load localized audio clip
        public async Task<AudioClip> LoadAudioClipAsync(LocalizedAsset<AudioClip> localizedClip)
        {
            localizedClip.LocaleOverride = AudioLocale;
            var handle = localizedClip.LoadAssetAsync();
            await handle.Task;
            return handle.Result;
        }

        public AudioClip LoadAudioClip(LocalizedAsset<AudioClip> localizedClip)
        {
            localizedClip.LocaleOverride = AudioLocale;
            AudioClip audioClip = localizedClip.LoadAsset();
            return audioClip;
        }

        public void LoadAudioClipAsync(LocalizedAsset<AudioClip> localizedClip, Action<AudioClip> onLoaded)
        {
            localizedClip.LocaleOverride = AudioLocale;
            var handle = localizedClip.LoadAssetAsync();

            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(operation.Result);
                }
                else
                {
                    Debug.LogError("Failed to load audio clip: " + operation.OperationException);
                    onLoaded?.Invoke(null);
                }
            };
        }
    }
}