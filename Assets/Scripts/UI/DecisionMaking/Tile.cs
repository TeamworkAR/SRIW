using System;
using Data.ScenarioSettings;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.DecisionMaking
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private int[] indexes;

        [SerializeField]
        private int currentIndex;

        public event Action TileUpdated;

        public int SetRandomIndex(int rightAnswer)
        {
            int randomValue = indexes[Random.Range(0, indexes.Length)];

            while (randomValue == rightAnswer) //checking if the random value is the correct one
            {
                randomValue = indexes[Random.Range(0, indexes.Length)];
            }

            currentIndex = randomValue;
            NotifyTileUpdated();
            return currentIndex;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public void SetCurrentIndex(int index)
        {
            currentIndex = index;
            NotifyTileUpdated();
        }

        public void CycleIndexes()
        {
            if(indexes.Length % 2 == 0) currentIndex = indexes[(currentIndex + 1) % indexes.Length];
            else
            {
                if (currentIndex == indexes[indexes.Length - 1]) currentIndex = indexes[0];
                else currentIndex++;
            }
            NotifyTileUpdated();
        }

        public string GetTileText()
        {
            return GetComponentInChildren<TextMeshProUGUI>().text;
        }

        public void SetTileText(string text)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        private void NotifyTileUpdated()
        {
            TileUpdated?.Invoke();
        }

    }
}
