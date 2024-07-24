using Core;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Conclusion
{
    public class ConclusionUI : BaseUICanvasGroup
    {
        [SerializeField] private TextMeshProUGUI m_TitleText = null;

        [SerializeField] private TextMeshProUGUI[] m_TileTitles = null;
        
        [SerializeField] private TextMeshProUGUI[] m_TileTexts = null;

        [SerializeField] private GameObject m_ButtonNext = null;

        [SerializeField] private RectangleConclusion m_Tile1 = null;
        [SerializeField] private RectangleConclusion m_Tile2 = null;
        [SerializeField] private RectangleConclusion m_Tile3 = null;

        private bool b_ConclusionDone = false;
        private bool nextButtonActivated = false;

        private const float k_BUTTON_ACTIVATION_WAIT = 4f;
        
        public override bool IsDone() => base.IsDone() && b_ConclusionDone;

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            b_ConclusionDone = false;
            nextButtonActivated = false;

            m_Tile1.ResetTile();
            m_Tile2.ResetTile();
            m_Tile3.ResetTile();

            m_ButtonNext.SetActive(false);
            
            m_ButtonNext.GetComponentInChildren<Button>().interactable = true;
            
            var extension = GameManager.Instance.ScenarioSettings.GetExtension<ConclusionUIExtenstion>();

            m_TitleText.text = extension.ConclusionTitle;

            for (int i = 0; i < m_TileTitles.Length; i++)
            {
                m_TileTitles[i].text = extension.TileTitles[i];
            }
            
            for (int i = 0; i < m_TileTexts.Length; i++)
            {
                m_TileTexts[i].text = extension.TileTexts[i];
            }
        }

        public void ConclusionsDone()
        {
            m_ButtonNext.GetComponentInChildren<Button>().interactable = false;
            b_ConclusionDone = true;
            Hide();
        }

        private void Update()
        {
            if (m_Tile1.isTileRotated || m_Tile2.isTileRotated || m_Tile3.isTileRotated) m_Tile1.DisableHandPointer();

            if (m_Tile1.isTileRotated && m_Tile2.isTileRotated && m_Tile3.isTileRotated && !nextButtonActivated)
            {
                StartCoroutine(Helpers.UI.COR_Cooldown(k_BUTTON_ACTIVATION_WAIT, null, () =>
                {
                    m_ButtonNext.SetActive(true);
                }));

                nextButtonActivated = true;
            }
        }
    }
}