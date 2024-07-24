using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class ClueCollectionExtension : ScenarioExtension
        {
            [SerializeField] private List<CharacterData.CharacterData> m_LeftGroupCharacters = new List<CharacterData.CharacterData>(0);

            [SerializeField] private List<CharacterData.CharacterData> m_RightGroupCharacters = new List<CharacterData.CharacterData>(0);

            [SerializeField] private List<PoliciesWrapper> m_PolicyWrappers = new List<PoliciesWrapper>(0);

            [SerializeField] private List<ThoughtsWrapper> m_ThoughtWrappers = new List<ThoughtsWrapper>(0);

            [SerializeField] private List<PortableThoughtsWrapper> m_Portables = new List<PortableThoughtsWrapper>(0);
            
            public List<CharacterData.CharacterData> LeftGroupCharacters => m_LeftGroupCharacters;

            public List<CharacterData.CharacterData> RightGroupCharacters => m_RightGroupCharacters;

            public List<ThoughtsWrapper> ThoughtWrappers => m_ThoughtWrappers;

            public List<PoliciesWrapper> PolicyWrappers => m_PolicyWrappers;

            public Sprite m_MainPolicyImage;

            [SerializeField] private LocalizedString m_PolicyTitle = null;

            public LocalizedString PolicyTitle => m_PolicyTitle;

            // TODO: Consider caching this result (e.g. a Dictionary)
            public List<Thought> GetThoughtsFor(CharacterData.CharacterData characterData, bool includePortables = false)
            {
                List<Thought> result = new List<Thought>(0);

                foreach (var thought in m_ThoughtWrappers.Where(thought => thought.Owner == characterData))
                {
                    result.AddRange(thought.Thoughts);
                }

                if (includePortables == false)
                {
                    return result;
                }
                
                foreach (var portableThoughtsWrapper in m_Portables.Where(portableThoughtsWrapper => portableThoughtsWrapper.Wrapper.Owner == characterData))
                {
                    result.AddRange(portableThoughtsWrapper.Wrapper.Thoughts);
                }

                return result;
            }

            public List<Thought> GetThoughtsAt(ThoughtsWrapper.LayoutPositions position, bool includePortables = false)
            {
                List<Thought> result = new System.Collections.Generic.List<Thought>(0);

                foreach (var thought in m_ThoughtWrappers.Where(thought => thought.LayoutPosition == position))
                {
                    result.AddRange(thought.Thoughts);
                }

                if (includePortables == false)
                {
                    return result;
                }
                
                foreach (var portableThoughtsWrapper in m_Portables.Where(portableThoughtsWrapper => portableThoughtsWrapper.Wrapper.LayoutPosition == position))
                {
                    result.AddRange(portableThoughtsWrapper.Wrapper.Thoughts);
                }

                return result;   
            }

            public CharacterData.CharacterData GetOwnerFor(Thought thought)
            {
                foreach (var thoughtWrapper in m_ThoughtWrappers)
                {
                    foreach (var thoughtWrapperThought in thoughtWrapper.Thoughts)
                    {
                        if (thoughtWrapperThought == thought)
                        {
                            return thoughtWrapper.Owner;
                        }
                    }
                }

                foreach (var portableThoughtsWrapper in m_Portables)
                {
                    foreach (var wrapperThought in portableThoughtsWrapper.Wrapper.Thoughts)
                    {
                        if (wrapperThought == thought)
                        {
                            return portableThoughtsWrapper.Wrapper.Owner;
                        }
                    }
                }

                return null;
            }

            public bool AreAllThoughtsUnlockedFor(CharacterData.CharacterData characterData) =>
                GetThoughtsFor(characterData).TrueForAll(t => t.IsUnlocked());

            public bool IsAnyThoughtUnlockedFor(CharacterData.CharacterData characterData) =>
                GetThoughtsFor(characterData, true).Exists(t => t.IsUnlocked());
            
            // TODO: Consider caching this result (e.g. a Dictionary)
            public List<Policy> GetPolicies()
            {
                List<Policy> result = new List<Policy>(0);
                foreach (var policy in m_PolicyWrappers)
                {
                    result.AddRange(policy.Policies);    
                }

                return result;
            }

            public bool AreAllPoliciesUnlocked() => GetPolicies().TrueForAll(p => p.IsUnlocked());

            public bool IsAnyPolicyUnlocked() => GetPolicies().Exists(p => p.IsUnlocked());

            public static void Unlock(PortableThoughtsWrapper portableThoughtsWrapper)
            {
                portableThoughtsWrapper.Wrapper.Thoughts.ForEach(t => t.UnlockWithoutNotify());
            }

            public static void Lock(PortableThoughtsWrapper portableThoughtsWrapper)
            {
                portableThoughtsWrapper.Wrapper.Thoughts.ForEach(t => t.Lock());
            }

            public void UnlockAll()
            {
                foreach (var policyWrapper in m_PolicyWrappers)
                {
                    policyWrapper.Policies.ForEach(p => p.UnlockWithoutNotify());
                }

                foreach (var thoughtWrapper in m_ThoughtWrappers)
                {
                    thoughtWrapper.Thoughts.ForEach(t => t.UnlockWithoutNotify());
                }
            }
            
            public void ResetAllClues()
            {
                foreach (var policyWrapper in m_PolicyWrappers)
                {
                    policyWrapper.Policies.ForEach(p => p.Lock());
                }

                foreach (var thoughtWrapper in m_ThoughtWrappers)
                {
                    thoughtWrapper.Thoughts.ForEach(t => t.Lock());
                }
            }

            // TODO: Consider adding a generic type to handle the actual data contained in the clue
            public abstract class UnlockableClue
            {
                public static event Action<UnlockableClue> onClueLocked = null; 
                public static event Action<UnlockableClue> onClueUnlocked = null; 
                
                private static HashSet<UnlockableClue> m_State = new HashSet<UnlockableClue>(0);

                public bool IsUnlocked() => m_State.Contains(this);

                public void Unlock()
                {
                    m_State.Add(this);
                    
                    onClueUnlocked?.Invoke(this);
                }

                public void UnlockWithoutNotify()
                {
                    m_State.Add(this);
                }

                public void Lock()
                {
                    if (m_State.Contains(this))
                    {
                        m_State.Remove(this);

                        onClueLocked?.Invoke(this);
                    }
                }
            }
            
            [Serializable]
            public class Policy : UnlockableClue
            {
                [SerializeField] private LocalizedString m_Text = null;

                [SerializeField] private LocalizedString m_Title = null;

                [SerializeField] private LocalizedString m_ClueText = null;
                
                public string GetText() => LocalizationManager.Instance.GetLocalizedValue(m_Text);

                public string GetTitle() => LocalizationManager.Instance.GetLocalizedValue(m_Title);

                public string GetClueText() => LocalizationManager.Instance.GetLocalizedValue(m_ClueText);
            }
            
            [Serializable]
            public class Thought : UnlockableClue
            {
                [SerializeField] private LocalizedString m_Text = null;

                [SerializeField] private LocalizedString m_ClueText = null;

                [SerializeField] private AudioClip m_ThoughtAudioClip = null;
                
                public string GetText() => LocalizationManager.Instance.GetLocalizedValue(m_Text);

                public string GetClueText() => LocalizationManager.Instance.GetLocalizedValue(m_ClueText);

                public AudioClip GetAudioClip() => m_ThoughtAudioClip;
            }

            /// <summary>
            /// Since Unity can't serialize Dictionary, let's use a wrapper class to map
            /// each character to their thoughts.
            /// This can be done also placing a list of thoughts into CharacterData
            /// but to me it seems more authoring-friendly to have all the clue collection unlockables
            /// inside this ScenarioSettingsExtension.
            /// Also, this data structure aids to position thoughts inside the clue book
            /// </summary>
            [Serializable]
            public class ThoughtsWrapper
            {
                public enum LayoutPositions
                {
                    None = 0,
                    UpperLeft = 1,
                    LowerLeft = 2,
                    Right = 3
                }

                [SerializeField, Tooltip("The positions of this list of thoughts in the clue book")] 
                private LayoutPositions m_LayoutPosition = LayoutPositions.None;
                
                [SerializeField] private CharacterData.CharacterData m_Owner = null;

                [SerializeField] private List<Thought> m_Thoughts = new List<Thought>(0);

                public CharacterData.CharacterData Owner => m_Owner;

                public List<Thought> Thoughts => m_Thoughts;

                public LayoutPositions LayoutPosition => m_LayoutPosition;
            }

            [Serializable]
            public class PoliciesWrapper
            {
                public enum LayoutPositions
                {
                    None = 0,
                    Left = 1,
                    Right = 2
                }

                [SerializeField, Tooltip("The positions of this list of policies in the clue book")]
                private LayoutPositions m_LayoutPosition = LayoutPositions.None;
                
                [SerializeField] private List<Policy> m_Policies = new List<Policy>(0);

                public List<Policy> Policies => m_Policies;

                public Texture policyImage;

            }
        }
    }
}