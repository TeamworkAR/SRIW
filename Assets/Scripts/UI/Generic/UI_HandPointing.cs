using UnityEngine;

public class UI_HandPointing : MonoBehaviour
{
    [SerializeField] private float m_MoveDistance = 0.5f; // Adjust this value for the desired movement distance.

    [SerializeField] private float m_MoveDuration = 1.0f;

    private Vector3 m_InitialLocalPosition = Vector3.zero;

    private Vector3 m_TargetUp = Vector3.zero;
    private Vector3 m_TargetDown = Vector3.zero;

    private float m_Timer = 0f;

    private void OnEnable()
    {
        m_InitialLocalPosition = transform.localPosition;

        m_TargetUp = m_InitialLocalPosition + Vector3.up * m_MoveDistance;
        m_TargetDown = m_InitialLocalPosition - Vector3.up * m_MoveDistance;

        m_Timer = 0f;
    }

    private void OnDisable()
    {
        transform.localPosition = m_InitialLocalPosition;

        m_Timer = 0f;
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;

        float t = Mathf.PingPong(m_Timer, m_MoveDuration) / m_MoveDuration;
        transform.localPosition = Vector3.Lerp(m_TargetUp, m_TargetDown, t);
    }
}
