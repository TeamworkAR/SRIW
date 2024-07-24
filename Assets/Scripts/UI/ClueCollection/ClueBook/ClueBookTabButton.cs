using UnityEngine;

public class ClueBookTabButton : MonoBehaviour
{
    [SerializeField] private GameObject m_ActiveViz = null;
    [SerializeField] private GameObject m_InactiveViz = null;

    public void SetActiveViz() 
    {
        m_ActiveViz.SetActive(true);
        m_InactiveViz.SetActive(false);
    }

    public void SetInactiveViz() 
    {
        m_ActiveViz.SetActive(false);
        m_InactiveViz.SetActive(true);
    }
}