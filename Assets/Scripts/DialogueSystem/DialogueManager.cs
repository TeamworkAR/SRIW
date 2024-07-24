using System.Collections;
using System.Collections.Generic;
using Core;
using Data.CharacterData;
using Data.DialogueData;
using Managers;
using NodeEditor.Graphs;
using UI;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : SingletonBehaviour<DialogueManager>
    {
        [SerializeField] private LayerMask m_OnDialogueMaks;
        [SerializeField] private LayerMask m_OffDialogueMaks;
        
        private DialogueData m_Data = null;

        private Dictionary<CharacterData, DialogueCharacter> m_Characters =
            new Dictionary<CharacterData, DialogueCharacter>(0);

        public Dictionary<CharacterData, DialogueCharacter> Characters => m_Characters;

        private Graph.GraphInstance m_GraphInstance = null;

        public bool IsDone => m_Running == null;

        private Coroutine m_Running = null;
        
        public void StartDialogue(DialogueData data)
        {
            if (m_Running != null)
            {
                return;
            }

            m_Data = data;

            MainGUI.Instance.Button_PauseToggle.Show();
            MainGUI.Instance.Button_SubtitleToggle.Show();

            m_Running = StartCoroutine(COR_DialogueLifeCycle());
        }

        public void EndDialogue()
        {
            if (m_Running == null)
            {
                return;
            }

            Camera.main.cullingMask = m_OffDialogueMaks;
            
            //Because we now wait for accessibilty to finish talking, this can be null
            m_GraphInstance?.Interrupt();

            //Because we now wait for accessibilty to finish talking, this can be null
            if (m_Characters != null)
            {
                foreach (var dialogueCharacter in m_Characters.Values)
                {
                    Destroy(dialogueCharacter.gameObject);
                }

                m_Characters.Clear();
            }

            m_Data = null;

            m_GraphInstance = null;
            
            // just to be sure
            MainGUI.Instance.MDialogueChoiceUI.Interrupt();
            if (MainGUI.Instance.MScreenCoverUI.IsOnScreen())
            {
                MainGUI.Instance.MScreenCoverUI.Hide();
            }
            else
            {
                MainGUI.Instance.MScreenCoverUI.Interrupt();   
            }

            MainGUI.Instance.MSubtitlesUI.ClearSubtitles();
            
            MainGUI.Instance.Button_PauseToggle.Hide();
            MainGUI.Instance.Button_SubtitleToggle.Hide();

            StopCoroutine(m_Running);
            m_Running = null;
        }

        private IEnumerator COR_DialogueLifeCycle()
        {
            MainGUI.Instance.MScreenCoverUI.Show();

            while (MainGUI.Instance.MScreenCoverUI.IsOnScreen() == false)
            {
                yield return null;
            }
            
            foreach (var characterData in m_Data.Characters)
            {
                m_Characters.Add(characterData,characterData.DialogueTemplate.GetInstance());
            }

            // Wait a frame to allow proper character loading else there can be conflicts with node graphs
            yield return null;

            // In older devices scene loading might be slow
            while (EnvironmentManager.Instance.IsSceneLoading)
            {
                yield return null;
            }
            
			EnvironmentManager.Instance.RecalcLightProbes();
			
            Camera.main.cullingMask = m_OnDialogueMaks;
            
            MainGUI.Instance.MScreenCoverUI.Hide();
            
            while (UAP_AccessibilityManager.IsSpeaking())
            {
                yield return new WaitForEndOfFrame();
            } 
            
            m_GraphInstance = m_Data.DialogueGraph.GetInstance(this);
            
            m_GraphInstance.Run();
            
            while (m_GraphInstance.IsDone == false)
            {
                yield return new WaitForSeconds(0.00001f);
            }
            
            MainGUI.Instance.MScreenCoverUI.Show();

            while (MainGUI.Instance.MScreenCoverUI.IsOnScreen() == false)
            {
                yield return new WaitForSeconds(0.00001f);
            }
            
            EndDialogue();
        }
    }
}