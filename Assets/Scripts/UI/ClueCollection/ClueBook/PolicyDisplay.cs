using System.Text;
using Data.ScenarioSettings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClueCollection.ClueBook
{
    public class PolicyDisplay : MonoBehaviour
    {
        [SerializeField] private RawImage m_Image = null;
        
        [SerializeField] private TextMeshProUGUI m_Text = null;
        
        [SerializeField] private GameObject m_LockedView = null;
        
        [SerializeField] private GameObject m_UnlockedView = null;
        
        public void FeedData(ScenarioSettings.ClueCollectionExtension.PoliciesWrapper wrapper)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var policyWrapper in wrapper.Policies)
            {
                if (policyWrapper.IsUnlocked())
                {
                    builder.AppendLine(policyWrapper.GetClueText() + "\n");
                }
            }

            if (builder.ToString().Length > 0)
            {
                m_LockedView.SetActive(false);
                m_UnlockedView.SetActive(true);
                m_Text.text = builder.ToString();

                m_Image.texture = wrapper.policyImage;

                // TODO: This script and ClueBookThoughtDisplay seem to share a lot of code, do something.
            }
            else
            {
                m_LockedView.SetActive(true);
                m_UnlockedView.SetActive(false);
            }
        }

        public void Clear()
        {
            m_Text.text = null;
            m_Image.texture = null;
            
            m_LockedView.SetActive(true);
            m_UnlockedView.SetActive(false);
            
            CharacterShowcase.ClearByOwner(this);
        }    
    }
}