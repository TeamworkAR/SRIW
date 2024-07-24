using System.Collections;
using Animation;
using Core;
using CrazyMinnow.SALSA;
using Data.ScriptableObjectVariables;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueSystem
{
    // TODO: Refactor
    public class DialogueCharacter : MonoBehaviour
    {
        private AudioSource m_AudioSource = null;
        
        private Animator m_Animator_Body = null;

        private Animator m_Animator_Lips = null;

        private CharacterCamera m_CharacterCamera = null;
        
        private Coroutine m_Running = null;

        private MoodController m_MoodController = null;

        private Eyes m_Eyes = null;
        
        public bool IsAnimating => m_Running != null;
        
        private void Awake()
        {

            m_Animator_Lips = GetComponentsInChildren<Animator>()[0];

            m_Animator_Body = GetComponentsInChildren<Animator>()[1];

            m_AudioSource = GetComponentInChildren<AudioSource>();

            m_CharacterCamera = GetComponent<CharacterCamera>();

            m_MoodController = GetComponent<MoodController>();

            m_Eyes = GetComponentInChildren<Eyes>();
        }

        public DialogueCharacter GetInstance()
        {
            return Instantiate(this, Vector3.zero, Quaternion.identity);
        }

        public void PlayLine(AnimatorOverrideController animatorOverrideController, AudioClip clip, CharacterCamera.CameraPositions cameraPosition, Consts.Moods.MoodData mood, LocalizedString subtitles, bool isLastDialogueNode)
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                
                m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, false);

                m_CharacterCamera.DeactivateCamera();
            }

            m_CharacterCamera.ActivateCamera(cameraPosition);

            if (mood.IsManual)
            {
                m_MoodController.HandleMoodManual(mood.MMood);
            }
            else
            {
                m_MoodController.HandleMoodRandom(mood);
            }
            
            m_Animator_Body.runtimeAnimatorController = animatorOverrideController;

            m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, true);

            m_AudioSource.Stop();
            m_AudioSource.clip = null;

            m_Running = StartCoroutine(COR_Talking(clip, subtitles, isLastDialogueNode));
        }

        public void PlayAnimation(AnimatorOverrideController animatorOverrideController, AnimatorOverrideController lipsAnimator, CharacterCamera.CameraPositions cameraPosition, Consts.Moods.MoodData mood, bool isLastDialogueNode, bool areLipsMoving)
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                
                m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, false);

                m_CharacterCamera.DeactivateCamera();
            }
            
            m_CharacterCamera.ActivateCamera(cameraPosition);

            if (areLipsMoving)
            {
                m_Animator_Lips.enabled = true;
            }

            else
            {
                m_Animator_Lips.enabled = false;
            }

            if (mood.IsManual)
            {
                m_MoodController.HandleMoodManual(mood.MMood);
            }
            else
            {
                m_MoodController.HandleMoodRandom(mood);
            }

            
            m_Animator_Body.runtimeAnimatorController = animatorOverrideController;
            
            m_Running = StartCoroutine(COR_Animating(isLastDialogueNode));
        }

        public void SetLookTarget(TransformVariable transformVariable)
        {
            m_Eyes.lookTarget = transformVariable.TransformValue;
        }

        private IEnumerator COR_Animating(bool isLastDialogueNode)
        {
            while (m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName(Consts.Animation.k_STATE_DIALOGUE_INACTIVE) == false)
            {
                yield return new WaitForSeconds(0.00001f);
            }
            
            m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, true);
            
            while (m_Animator_Body.GetCurrentAnimatorStateInfo(0).IsName(Consts.Animation.k_STATE_DIALOGUE_INACTIVE))
            {
                yield return new WaitForSeconds(0.00001f);
            }

            float clipLength = m_Animator_Body.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float t = 0f;

            while (t < clipLength)
            {
                // Oder matters, placing the yield after the timer increment leads to a slight loop in the animation
                yield return new WaitForSeconds(0.00001f);
                
                //Time.deltaTime still returns a finite number when Time.Timescale = 0
                if (Time.timeScale != 0f)
                    t += Time.deltaTime;
            }

            if (!isLastDialogueNode)
            {
                m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, false);

                m_Animator_Lips.Rebind();

                m_Animator_Lips.enabled = false;

                m_CharacterCamera.DeactivateCamera();
            }
            
            m_Running = null;
        }

        private IEnumerator COR_Talking(AudioClip clip, LocalizedString subtitles, bool isLastDialogueNode)
        {
            // Waiting one frame in order to make Amplitude for SALSA update itself
            // Without this line, having a character playing two lines one after the another would result in broken lip sync
            yield return null;

            while (UAP_AccessibilityManager.IsSpeaking())
            {
                yield return new WaitForEndOfFrame();
            }
            
            m_AudioSource.clip = clip;
            
            MainGUI.Instance.MSubtitlesUI.ShowSubtitle(LocalizationManager.Instance.GetLocalizedValue(subtitles));
            
            // TODO: Maybe it would be better to author an AnimationEvent to properly coordinate animations and audio
            m_AudioSource.Play();
            bool paused = false;
            while (m_AudioSource.isPlaying || paused || Time.timeScale == 0)
            {
                if (Time.timeScale == 0 && m_AudioSource.isPlaying)
                {
                    m_AudioSource.Pause();
                    paused = true;
                }
                if(paused && Time.timeScale > 0)
                {
                    m_AudioSource.UnPause();
                    paused = false;
                }
                yield return new WaitForEndOfFrame();
            }

            if (!isLastDialogueNode)
            {
                m_Animator_Body.SetBool(Consts.Animation.k_BOOL_DIALOGUE_ANIMATE, false);

                m_CharacterCamera.DeactivateCamera();
            }
            
            MainGUI.Instance.MSubtitlesUI.ClearSubtitles();
            
            m_Running = null;
        }
    }
}