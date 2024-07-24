#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

namespace Animation
{
    /// <summary>
    /// We require a special setup for our models (i.e. the animator has to be attached on a child GameObject called "Armature")
    /// This script ensure the right setup directly in the base prefab instead in all of its derivates.
    /// </summary>
    public class AnimationSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private AnimatorController m_AnimatorController;  
#endif
        
        [SerializeField] private AnimatorOverrideController m_OverrideIfAny = null;
        [SerializeField] private AnimatorOverrideController m_OverrideIfAny_Lips = null;

        private Animator m_Animator_Body = null;
        private Animator m_Animator_Lips = null;


        private void Awake()
        {
            m_Animator_Lips = GetComponentsInChildren<Animator>()[0];
            m_Animator_Body = GetComponentsInChildren<Animator>()[1];
        
            if (m_Animator_Body == null)
            {
                Debug.LogError($"Can't find a {nameof(Animator)} attached on any child of {gameObject.name}. Animations won't play properly");    
            }
            
            if (m_OverrideIfAny != null)
            {
                m_Animator_Body.runtimeAnimatorController = m_OverrideIfAny;
            } 

            if(m_OverrideIfAny_Lips != null)
            {
                m_Animator_Lips.runtimeAnimatorController = m_OverrideIfAny_Lips;
            }
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            Animator animator = GetComponentInChildren<Animator>();

            if (animator != null)
            {
                animator.runtimeAnimatorController = m_AnimatorController;
            }
            else
            {
                Debug.LogWarning($"Can't find an {nameof(Animator)} on {gameObject.name}. Make sure it's a base prefab");   
            }
        }
#endif
    }
}