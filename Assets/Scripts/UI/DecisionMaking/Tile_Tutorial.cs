using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Tile_Tutorial : MonoBehaviour
{
    // private Dictionary<int, KeyValuePair<LocalizedString, Sprite>> m_Entries = new Dictionary<int, KeyValuePair<LocalizedString, Sprite>>();
    //
    // [SerializeField] private Sprite m_ActNow = null;
    // [SerializeField] private Sprite m_Report = null;
    // [SerializeField] private Sprite m_LetGo = null;
    //
    // [SerializeField] private LocalizedString m_ActNowString = null;
    // [SerializeField] private LocalizedString m_ReportString = null;
    // [SerializeField] private LocalizedString m_LetGoString = null;
    //
    // [SerializeField] private Image image = null;
    // [SerializeField] private TextMeshProUGUI text = null;
    //
    // private int currentIndex = 0;
    //
    // private void Start()
    // {
    //     m_Entries.Add(0, new KeyValuePair<LocalizedString, Sprite>(m_ActNowString, m_ActNow));
    //     m_Entries.Add(1, new KeyValuePair<LocalizedString, Sprite>(m_ReportString, m_Report));
    //     m_Entries.Add(2, new KeyValuePair<LocalizedString, Sprite>(m_LetGoString, m_LetGo));
    //
    //     image.sprite = m_Entries[currentIndex].Value;
    //     text.text = m_Entries[currentIndex].Key.GetLocalizedString();
    // }
    //
    // private IEnumerator COR_NextValueLifeCycle()
    // {
    //     while (true)
    //     {
    //         yield return Helpers.UI.COR_Rotate(gameObject, new Vector3(0f, 90f, 0f), 0.2f);
    //
    //         UpdateIndex();
    //
    //         yield return Helpers.UI.COR_Rotate(gameObject, Vector3.zero, 0.2f);
    //
    //         yield return new WaitForSeconds(1.0f);
    //     }
    // }
    //
    // private void UpdateIndex()
    // {
    //     currentIndex++;
    //
    //     if (currentIndex >= m_Entries.Count) currentIndex = 0;
    //
    //     image.sprite = m_Entries[currentIndex].Value;
    //     text.text = m_Entries[currentIndex].Key.GetLocalizedString();
    // }
    //
    // public void StartRotateCoroutine()
    // {
    //     StartCoroutine(COR_NextValueLifeCycle());
    // }
}
