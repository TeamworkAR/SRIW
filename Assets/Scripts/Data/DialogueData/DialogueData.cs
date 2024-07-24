using System.Collections.Generic;
using NodeEditor.Graphs;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.DialogueData
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(DialogueData))]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private Graph m_DialogueGraph = null;
        
        [SerializeField] private List<CharacterData.CharacterData> m_Characters = new List<CharacterData.CharacterData>(0);
        [SerializeField] private LocalizedString m_AccessibilityDescription;
        
        public Graph DialogueGraph => m_DialogueGraph;

        public List<CharacterData.CharacterData> Characters => m_Characters;
        public LocalizedString AccessibilityDescription => m_AccessibilityDescription;
    }
}