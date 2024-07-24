using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundUI : BaseUICanvasGroup
{
    [SerializeField] public RawImage m_BackgroundImage = null;

    [SerializeField] private YellowBackground m_YellowBackground = null;
    
    protected override void OnShowStart()
    {
        base.OnShowStart();
    }

    protected override void OnShowCompleted()
    {
        base.OnShowCompleted();
        
        m_Behaviour.interactable = false;
        m_Behaviour.blocksRaycasts = false;
    }

    protected override void OnHideCompleted()
    {
        base.OnHideCompleted();
        
        m_Behaviour.interactable = false;
        m_Behaviour.blocksRaycasts = false;
    }

    public void ActivateYellowBackground()
    {
        m_YellowBackground.Show();
    }

    public void DeactivateYellowBackground()
    {
        m_YellowBackground.Hide();
    }
}
