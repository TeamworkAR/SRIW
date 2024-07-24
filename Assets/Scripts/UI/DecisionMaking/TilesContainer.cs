using System.Collections.Generic;
using Data.ScenarioSettings;
using Managers;
using UI.Generic;
using UnityEngine;
using static Core.Helpers.UI;

namespace UI.DecisionMaking
{
    public sealed class TilesContainer : MonoBehaviour
    {
        [SerializeField] private CarouselCounterContainer m_CarouselCounterContainer = null;

        [SerializeField] private RectTransform m_Container = null;

        [SerializeField] private TileTemplate m_TileTemplate = null;
        
        [SerializeField] private int m_AccessibleSortOrder = 0;

        private List<TileTemplate> m_TileInstances = new List<TileTemplate>(0);

        private ScenarioSettings.DecisionMakingExtension m_Data = null;

        //private List<ScenarioSettings.DecisionMakingExtension.Entry> m_Entries = null;

        private CyclingList<ScenarioSettings.DecisionMakingExtension.Entry> m_Entries = null;

        private int m_TileIndex = int.MinValue;

        public int TileIndex => m_TileIndex;

        public int EntryIndex => m_Entries.Idx;

        public ScenarioSettings.DecisionMakingExtension.Entry Current() => m_Entries.GetCurrent();

        public void Clear()
        {
            m_TileInstances.ForEach(t => Destroy(t.gameObject));
            m_TileInstances.Clear();

            m_CarouselCounterContainer.Dispose();
        }

        public void FeedData(ScenarioSettings.DecisionMakingExtension data, int index)
        {
            m_Data = data;

            m_TileIndex = index;

            m_Entries = new CyclingList<ScenarioSettings.DecisionMakingExtension.Entry>(m_Data.Tiles[m_TileIndex].Entries);
            foreach (var entry in m_Entries.Content)
            {
                var tile = Instantiate(m_TileTemplate, m_Container);
                m_TileInstances.Add(tile);

                tile.FeedData(entry);
                tile.AccessibleLabel.m_ManualPositionOrder = m_AccessibleSortOrder;
            }

            m_CarouselCounterContainer.Init(m_Entries.Count, true);
            m_CarouselCounterContainer.SetChevronsActiveState(true);
            m_CarouselCounterContainer.Select(m_Entries.Idx);

            UpdateSortingOrder();
        }

        public void ShowCorrectResult()
        {
            while (m_Entries.GetCurrent().IsRightEntry == false)
            {
                m_Entries.Next();
            }

            UpdateSortingOrder();
        }

        public void ShowResult() => m_TileInstances[m_Entries.Idx].ShowResult();

        public void ResetViz() => m_TileInstances[m_Entries.Idx].ResetViz();

        public void EnableInteraction() => m_CarouselCounterContainer.EnableInteraction(m_Entries.Idx);

        public void DisableInteraction() => m_CarouselCounterContainer.DisableInteraction();

        private void UpdateSortingOrder()
        {
            int count = m_TileInstances.Count;

            // Set the layer of the entry at m_EntryIndex to the collection count
            m_TileInstances[m_Entries.Idx].SetLayer(count);
            m_TileInstances[m_Entries.Idx].SetActiveState();

            // Set the layers for the entries before m_EntryIndex
            for (int i = m_Entries.Idx - 1; i >= 0; i--)
            {
                m_TileInstances[i].SetLayer(count - (m_Entries.Idx - i));
                m_TileInstances[i].SetInactiveState();
            }

            // Set the layers for the entries after m_EntryIndex
            for (int i = m_Entries.Idx + 1; i < count; i++)
            {
                m_TileInstances[i].SetLayer(count - (i - m_Entries.Idx));
                m_TileInstances[i].SetInactiveState();
            }

            m_CarouselCounterContainer.Select(m_Entries.Idx);
        }

        public void Next() 
        {
            m_Entries.Next();
            
            UpdateSortingOrder();
            m_TileInstances[m_Entries.Idx].AccessibleLabel.Select();

            //AudioManager.Instance.DoSfx(GameManager.Instance.DevSettings.DecisionMakingTilesSFX);
        }

        public void Previous()
        {
            m_Entries.Previous();

            UpdateSortingOrder();
            m_TileInstances[m_Entries.Idx].AccessibleLabel.Select();

            //AudioManager.Instance.DoSfx(GameManager.Instance.DevSettings.DecisionMakingTilesSFX);
        }
    }
}