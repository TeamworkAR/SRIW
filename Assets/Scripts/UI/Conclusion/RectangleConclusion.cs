using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleConclusion : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI m_Text = null;
    [SerializeField] private RectTransform m_Title = null;
    [SerializeField] private GameObject m_Frame = null;
    [SerializeField] private GameObject m_TickCorrect = null;
    [SerializeField] private GameObject m_HandPointing = null;

    public UnityEvent OnPanelRotated;
    public bool isTileRotated = false;
    private static Coroutine m_Running;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isTileRotated && m_Running == null) TryStartLifecycle();
    }

    private IEnumerator COR_RotateTileLifeCycle()
    {
        isTileRotated = true;

        if (m_HandPointing != null) DisableHandPointer();

        yield return Helpers.UI.COR_RotateHorizontally(gameObject, 90f, 0.2f);

        m_Title.anchorMax = new Vector2(0.95f, 0.95f);
        m_Title.anchorMin = new Vector2(0.05f, 0.75f);

        m_Text.gameObject.SetActive(true);
        m_Text.enabled = true;
        OnPanelRotated?.Invoke();
        
        yield return Helpers.UI.COR_RotateHorizontally(gameObject, 0f, 0.2f);
        
        m_Frame.SetActive(true);

        yield return Helpers.UI.COR_FillImage(m_Frame.GetComponentInChildren<Image>(), 0f, 1f, 3f);

        m_TickCorrect.SetActive(true);

        yield return Helpers.UI.COR_Scale(m_TickCorrect.transform, new Vector3(0, 1, 1), new Vector3(1, 1, 1), 0.1f);

        TryStopLifecycle();
    }

    public void TryStartLifecycle()
    {
        if (m_Running != null)
        {
            return;
        }

        m_Running = StartCoroutine(COR_RotateTileLifeCycle());
    }

    private void TryStopLifecycle()
    {
        if (m_Running == null)
        {
            return;
        }

        StopCoroutine(m_Running);

        m_Running = null;
    }

    public void ResetTile()
    {
        m_Title.anchorMax = new Vector2(0.95f, 0.6f);
        m_Title.anchorMin = new Vector2(0.05f, 0.4f);

        m_Text.gameObject.SetActive(false);
        m_Text.enabled = false;

        isTileRotated = false;

        m_Frame.SetActive(false);
        m_TickCorrect.SetActive(false);

        if (m_HandPointing != null) 
        {
           EnableHandPointer();
        }
    }

    public void DisableHandPointer()
    {
        m_HandPointing.SetActive(false);
    }

    public void EnableHandPointer()
    {
        m_HandPointing.SetActive(true);
    }



}
