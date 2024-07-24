using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Debugging
{
     /// <summary>
     /// Simple UI for debugging the basic ScormManager behaviours.
     /// <see cref="ScormManager"/>
     /// </summary>
     public class ScormDebugUI : MonoBehaviour
     {
         [SerializeField] private Button logButton;
         [SerializeField] private Button localeButton;
         [SerializeField] private Button bookmarkButton;
         [SerializeField] private Button suspendButton;
         [SerializeField] private Button completeButton;
         [SerializeField] private Button finishButton;
         [SerializeField] private Button resetButton;
         
         public List<string> languages = new(new []{"en","fr"});
         public List<string> potentialBookmarks = new(new[]{"bookmark1","bookmark2"});
         public List<SuspendKeyVal> suspendKeyVals = new();
         
         private int langInt = 0;
         private int bookmarkInt = 0;
         
         void Start()
         {
             if (ScormManager.Instance.Initialized) ManagerReady();
             else ScormManager.Instance.ScormManagerReady += ManagerReady;
             logButton.onClick.AddListener(DebugLogClick);
             localeButton.onClick.AddListener(LocaleClick);
             bookmarkButton.onClick.AddListener(BookmarkClick);
             suspendButton.onClick.AddListener(SuspendClick);
             completeButton.onClick.AddListener(CompleteClick);
             finishButton.onClick.AddListener(FinishClick);
             resetButton.onClick.AddListener(ResetClick);
         }

         private void ManagerReady()
         {
             var hydratedSuspendData = new List<SuspendKeyVal>();
             foreach (var key in ScormManager.Instance.CustomDataKeys)
             {
                 hydratedSuspendData.Add(new SuspendKeyVal(key, ScormManager.Instance.GetCustomString(key)));
             }
             suspendKeyVals.Clear();
             suspendKeyVals.AddRange(hydratedSuspendData);
         }
         
         private void OnDestroy()
         {
             logButton.onClick.RemoveListener(DebugLogClick);
             localeButton.onClick.RemoveListener(LocaleClick);
             bookmarkButton.onClick.RemoveListener(BookmarkClick);
             suspendButton.onClick.RemoveListener(SuspendClick);
             completeButton.onClick.RemoveListener(CompleteClick);
             finishButton.onClick.RemoveListener(FinishClick);
             resetButton.onClick.RemoveListener(ResetClick);
             ScormManager.Instance.ScormManagerReady -= ManagerReady;
         }
     
         private void DebugLogClick()
         {
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }
     
         private void LocaleClick()
         {
             if (languages.Count <= 0) return;
             ScormManager.Instance.Locale = languages[langInt];
             langInt = (langInt + 1) % languages.Count;
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }

         private void BookmarkClick()
         {
             if (potentialBookmarks.Count <= 0) return;
             ScormManager.Instance.Bookmark = potentialBookmarks[bookmarkInt];
             bookmarkInt = (bookmarkInt + 1) % potentialBookmarks.Count;
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }
         
         private void SuspendClick()
         {
             foreach (var suspendItem in suspendKeyVals)
             {
                 ScormManager.Instance.StoreCustomData(suspendItem.key, suspendItem.value);
             }
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }
     
         private void CompleteClick()
         {
             ScormManager.Instance.SetCourseCompletion(true);
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }

         private void FinishClick()
         {
             ScormManager.Instance.FinishCourse();
             Debug.Log(ScormManager.Instance.GetDebugString(true));
         }

         private void ResetClick()
         {
             throw new NotImplementedException();
         }
     }

     [Serializable]
     public struct SuspendKeyVal
     {
         public SuspendKeyVal(string key, string value)
         {
             this.key = key;
             this.value = value;
         }
         
         public string key;
         public string value;
     }
}