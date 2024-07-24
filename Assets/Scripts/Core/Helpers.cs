using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    /// <summary>
    /// Contains classes with useful methods that can be reused throughout the code base
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Contains useful methods to handle UI features like animations
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// Fades a canvas group's alpha value from a start value to an end value over a specified duration.
            /// </summary>
            /// <param name="canvasGroup">The canvas group to apply the fade effect to.</param>
            /// <param name="startValue">The starting alpha value.</param>
            /// <param name="endValue">The ending alpha value.</param>
            /// <param name="duration">The duration over which the fade effect should occur.</param>
            /// <returns>An IEnumerator for coroutine management.</returns>
            public static IEnumerator COR_Fade(CanvasGroup canvasGroup, float startValue, float endValue, float duration, Action onCompleted = null, Action onStart = null, bool canPause = true)
            {
                onStart?.Invoke();
                
                float t = 0;
                canvasGroup.alpha = startValue;

                
                while (t <= duration)
                {
                    canvasGroup.alpha = Mathf.Lerp(startValue, endValue, t / duration);
                    if (canPause)
                    {
                        //Time.deltaTime still returns a finite number when Time.Timescale = 0
                        if (Time.timeScale != 0f)
                            t += Time.deltaTime;
                        yield return new WaitForSeconds(0.00001f);
                    }
                    else
                    {
                        t += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }

                canvasGroup.alpha = endValue;
                
                onCompleted?.Invoke();
            }

            /// <summary>
            /// Calculates the estimated time to read a given text.
            /// </summary>
            /// <param name="text">The text to be read.</param>
            /// <returns>The estimated read time in minutes.</returns>
            public static float GetReadTime(string text)
            {
                // int wordCount = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
                // float wordsPerMinute = 200; // Average reading speed
                // return (wordCount / wordsPerMinute) + 2f;
                return Mathf.Max(text.Length * 0.1f, 4f);
            }

            /// <summary>
            /// Executes a cooldown timer with optional actions before and after the wait.
            /// </summary>
            /// <param name="t">The duration of the cooldown in seconds.</param>
            /// <param name="onPreWait">An optional action to be invoked before the wait starts.</param>
            /// <param name="onPostWait">An optional action to be invoked after the wait is over.</param>
            /// <returns>An IEnumerator for coroutine management.</returns>
            public static IEnumerator COR_Cooldown(float t, Action onPreWait = null, Action onPostWait = null)
            {
                onPreWait?.Invoke();
                while (t >= 0)
                {
                    yield return new WaitForSeconds(0.00001f);
                    //Time.deltaTime still returns a finite number when Time.Timescale = 0
                    if (Time.timeScale != 0f)
                        t -= Time.deltaTime;
                }
                onPostWait?.Invoke();
            }

            /// <summary>
            /// Scales a transform from a start scale to an end scale over a specified duration.
            /// </summary>
            /// <param name="transform">The transform to apply the scaling effect to.</param>
            /// <param name="startScale">The starting scale as a Vector3.</param>
            /// <param name="endScale">The ending scale as a Vector3.</param>
            /// <param name="duration">The duration over which the scaling should occur.</param>
            /// <returns>An IEnumerator for coroutine management.</returns>
            public static IEnumerator COR_Scale(Transform transform, Vector3 startScale, Vector3 endScale, float duration)
            {
                transform.localScale = startScale;

                float t = 0;

                while (t <= duration)
                {
                    transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
                    
                    //Time.deltaTime still returns a finite number when Time.Timescale = 0
                    if (Time.timeScale != 0f)
                        t += Time.deltaTime;

                    yield return new WaitForSeconds(0.00001f);
                }

                transform.localScale = endScale;
            }

            public static IEnumerator COR_FillImage(Image image, float startValue = 0f, float endValue = 1f, float duration = 1f)
            {
                float t = 0f;

                image.fillAmount = startValue;

                while (t <= duration)
                {
                    image.fillAmount = Mathf.Lerp(startValue, endValue, t / duration);
                    
                    //Time.deltaTime still returns a finite number when Time.Timescale = 0
                    if (Time.timeScale != 0f)
                        t += Time.deltaTime;
                    
                    yield return new WaitForSeconds(0.00001f);
                }

                image.fillAmount = endValue;
            }

            public static IEnumerator COR_Composite(List<IEnumerator> ops)
            {
                foreach (var cor in ops)
                {
                    yield return cor;
                }
            }

            public static IEnumerator COR_Rotate(List<GameObject> rotating, Vector3 euler, float duration)
            {
                float t = 0f;

                Dictionary<GameObject, Quaternion> startingRotations = new Dictionary<GameObject, Quaternion>(0);
                
                foreach (var gameObject in rotating)
                {
                    startingRotations.Add(gameObject,gameObject.transform.rotation);
                }

                Quaternion targetRotation = Quaternion.Euler(euler);

                while (t <= duration)
                {
                    foreach (var gameObject in rotating)
                    {
                        gameObject.transform.rotation = Quaternion.Lerp(startingRotations[gameObject], targetRotation, t / duration);
                    }
                    
                    //Time.deltaTime still returns a finite number when Time.Timescale = 0
                    if (Time.timeScale != 0f)
                        t += Time.deltaTime;

                    yield return new WaitForSeconds(0.00001f);
                }

                foreach (var gameObject in rotating)
                {
                    gameObject.transform.rotation = targetRotation;
                }
            }

            public static IEnumerator COR_RotateHorizontally(GameObject tile, float angle, float duration)
            {
                float t = 0f;

                Quaternion startRotation = tile.transform.rotation;

                Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

                while (t <= duration)
                {
                    tile.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t / duration);
                    
                    //Time.deltaTime still returns a finite number when Time.Timescale = 0
                    if (Time.timeScale != 0f)
                        t += Time.deltaTime;

                    yield return new WaitForSeconds(0.00001f);
                }

                tile.transform.rotation = targetRotation;
            }

            public class CyclingList<T>
            {
                private IList<T> m_Content;

                private int m_idx = 0;

                public IList<T> Content => m_Content;

                public CyclingList(IList<T> content)
                {
                    m_Content = content;
                }

                public T GetCurrent() => m_Content[m_idx];

                public int Count => m_Content.Count;

                public bool IsLast => m_idx == m_Content.Count - 1;

                public int Idx => m_idx;

                public void Next(bool cycle = true)
                {
                    if (m_idx < m_Content.Count - 1)
                    {
                        m_idx++;

                        return;
                    }

                    if (cycle)
                    {
                        m_idx = 0;
                    }
                }

                public void Previous(bool cycle = true)
                {
                    if (m_idx > 0)
                    {
                        m_idx--;

                        return;
                    }

                    if (cycle)
                    {
                        m_idx = m_Content.Count - 1;
                    }
                }
            }
        }

        public static class Audio
        {
            public static float GetAudioClipLenght(AudioClip clip)
            {
                return clip.length;
            }
        }
    }
}