using Data.ScenarioSettings;
using TMPro;
using UnityEngine;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookThoughtDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_InsightText = null;
        [SerializeField] private TextMeshProUGUI m_ClueText = null;
        
        public void FeedData(ScenarioSettings.ClueCollectionExtension.Thought thought)
        {
            m_InsightText.text = thought.GetText();

            m_ClueText.text = thought.GetClueText();
        }
    }
}