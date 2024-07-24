using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Core;
using TMPro;
using UnityEngine;

namespace UI.Subtitles
{
    public class SubtitlesUI : BaseUICanvas
    {
        [SerializeField] private TextMeshProUGUI m_Text = null;

        private bool b_SubtitlesOn = true;

        private List<(string,float)> m_SubtitlesChain = new List<(string, float)>(0);
        
        public bool SubtitlesOn => b_SubtitlesOn;

        public void ShowSubtitle(string text)
        {
            m_SubtitlesChain = ParseSubtitle(text);

            if (m_SubtitlesChain.Count > 0)
            {
                UICoroutinesHandler.Instance.TryStartCoroutine(this, COR_ShowSubtitlesChain());
            }
            else
            {
                m_Text.text = text;   
            }
            
            if (b_SubtitlesOn == false)
            {
                return;
            }

            Show();
        }

        public void ClearSubtitles()
        {
            m_Text.text = string.Empty;

            m_SubtitlesChain.Clear();

            UICoroutinesHandler.Instance.TryStopCoroutine(this);

            Hide();
        }

        public void ToggleSubtitles()
        {
            b_SubtitlesOn = !b_SubtitlesOn;

            if (b_SubtitlesOn == false)
            {
                Hide();
            }
            else
            {
                if (string.IsNullOrEmpty(m_Text.text) == false)
                {
                    Show();
                }
            }
        }

        private IEnumerator COR_ShowSubtitlesChain()
        {
            foreach (var chainEntry in m_SubtitlesChain)
            {
                m_Text.text = chainEntry.Item1;

                yield return Helpers.UI.COR_Cooldown(chainEntry.Item2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private static List<(string, float)> ParseSubtitle(string input)
        {
            List<(string, float)> subtitles = new List<(string, float)>();

            // Regular expression pattern to match time ranges and subtitles
            string pattern = @"\[(\d+(\.\d+)?)-(\d+(\.\d+)?)\]\s*([^\[\]]+)";

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                try
                {
                    if (!IsSubtitleFormatValid(match))
                    {
                        throw new FormatException("Invalid subtitle format.");
                    }

                    // Extracting start and end times
                    string startTime = match.Groups[1].Value;
                    string endTime = match.Groups[3].Value;

                    // Parsing time and validating format
                    float startInSeconds, endInSeconds;
                    if (!ParseTime(startTime,endTime, out float result))
                    {
                        throw new FormatException("Invalid time format.");
                    }

                    // Validation: Start time should be less than end time
                    if (result < 0)
                    {
                        throw new FormatException("Start time should be less than end time.");
                    }

                    // Extracting the subtitle text
                    string text = match.Groups[5].Value.Trim();

                    subtitles.Add((text, result));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing subtitle: {ex.Message}");
                    
                    return subtitles;
                }
            }

            return subtitles;
        }

        private static bool IsSubtitleFormatValid(Match match)
        {
            // Check if the match has 3 groups: start time, end time, and subtitle text
            return match.Success;
        }

        private static bool ParseTime(string startTime, string endTime, out float result)
        {
            result = float.Parse(endTime, CultureInfo.InvariantCulture) - float.Parse(startTime, CultureInfo.InvariantCulture);;
            
            return true;
        }
    }
}