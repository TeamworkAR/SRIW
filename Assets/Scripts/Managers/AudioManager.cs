using System;
using System.Collections;
using UnityEngine;
using Core;
using UnityEngine.UI;

namespace Managers
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
		[Header("Volume toggling")]
		[SerializeField] private Image m_iconVolume;
		[SerializeField] private Sprite m_spriteVolumeOn, m_spriteVolumeOff;
		
		[Header("Music management")]
		[SerializeField] private float m_transitionTime = 3;
		[SerializeField] private AudioSource m_audioSource_musicA;
		[SerializeField] private AudioSource m_audioSource_musicB;
		private bool m_musicIsTransitioning = false;
		
		[Header("Thought collection")]
		[SerializeField] private AudioSource audioSource_thoughts;

		[Header("Decision Making")] [SerializeField]
		private AudioSource m_DecisionMakingAudioSource = null;

		[Header("SFXs")] [SerializeField] private AudioSource m_SfxSource = null;

		private bool m_isPaused = false;
		
		
		
        public void ToggleVolume()
        {
			if (AudioListener.volume == 0)
			{
				AudioListener.volume = 1;
				m_iconVolume.sprite = m_spriteVolumeOn;
			}
			else
			{
				AudioListener.volume = 0;
				m_iconVolume.sprite = m_spriteVolumeOff;
			}
        }

        private void Update()
        {
	        if (Time.timeScale == 0f && !m_isPaused)
	        {
		        PauseAudio();
	        }
	        else if (m_isPaused && Mathf.Approximately(Time.timeScale ,1f))
	        {
		        PlayAudio();
	        }
        }

        private void PauseAudio()
        {
	        m_isPaused = true;
	        m_audioSource_musicA.Pause();
	        m_audioSource_musicB.Pause();
        }
        private void PlayAudio()
        {
	        m_isPaused = false;
	        m_audioSource_musicA.Play();
	        m_audioSource_musicB.Play();
        }

        public void PlayMusic(AudioClip newMusic, bool immediate = false)
		{
			if (immediate || m_musicIsTransitioning)
			{
				m_musicIsTransitioning = false;
				
				// TODO: Done in order to handle music changes requests during a transition. A more refined approach will be needed.
				StopAllCoroutines();
				
				AssignNewMusic(m_audioSource_musicA,newMusic);
				AssignNewMusic(m_audioSource_musicB,null);

				m_audioSource_musicA.volume = m_audioSource_musicA.clip == null ? 0f : 1f;
				m_audioSource_musicB.volume = m_audioSource_musicB.clip == null ? 0f : 1f;
				
				return;
			}

			if (!m_musicIsTransitioning)
			{
				if (!m_audioSource_musicA.isPlaying && !m_audioSource_musicB.isPlaying) // no one is playing
				{		
					AssignNewMusic(m_audioSource_musicA, newMusic);
					StartCoroutine(StartFade(m_audioSource_musicA, m_transitionTime, 1f));
				}
				else // Activate Crossfade
				{
					if (m_audioSource_musicA.isPlaying) // A is playing
					{
						AssignNewMusic(m_audioSource_musicB, newMusic);
						StartCoroutine(StartFade(m_audioSource_musicB, m_transitionTime, 1f));
						StartCoroutine(StartFade(m_audioSource_musicA, m_transitionTime, 0f));
					}
					else  // B is playing
					{
						AssignNewMusic(m_audioSource_musicA, newMusic);
						StartCoroutine(StartFade(m_audioSource_musicA, m_transitionTime, 1f));
						StartCoroutine(StartFade(m_audioSource_musicB, m_transitionTime, 0f));
					}
				}			
			}
		}			
		
		private void AssignNewMusic(AudioSource targetAudioSource, AudioClip targetClip)
		{
			if (targetAudioSource.clip == targetClip)
			{
				return;
			}

			targetAudioSource.Stop();
			targetAudioSource.clip = null;
			
			if (targetClip == null)
			{
				targetAudioSource.volume = 0f;
				
				return;
			}

			targetAudioSource.volume = 1f;
			targetAudioSource.clip = targetClip;
			targetAudioSource.Play();
		}
		
		private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
		{			
			m_musicIsTransitioning = true;
			
			float currentTime = 0;
			float start = audioSource.volume;			
			
			while (currentTime < duration)
			{				
				currentTime += Time.deltaTime;
				audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
				yield return null;
			}

			if (Mathf.Approximately(targetVolume, 0f))
			{
				audioSource.Stop();
				audioSource.clip = null;
			}
		
			m_musicIsTransitioning = false;
		}
		
		public void PlayThought(AudioClip thoughtClip)
		{
			audioSource_thoughts.Stop();
			audioSource_thoughts.clip = thoughtClip;
			audioSource_thoughts.Play();
		}

		public void StopThoughts()
        {
			audioSource_thoughts.Stop();
        }

		public void PlayDecisioMakingClip(AudioClip clip)
		{
			m_DecisionMakingAudioSource.Stop();
			m_DecisionMakingAudioSource.clip = clip;
			m_DecisionMakingAudioSource.Play();
		}

		public void DoSfx(AudioClip clip)
		{
			m_SfxSource.clip = clip;
			
			m_SfxSource.Play();
		}
    }
}