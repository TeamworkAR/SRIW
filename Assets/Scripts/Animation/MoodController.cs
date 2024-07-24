using System;
using System.Linq;
using Core;
using CrazyMinnow.SALSA;
using UnityEngine;

namespace Animation
{
    public class MoodController : MonoBehaviour
    {
        private Emoter m_Emoter = null;

        private void Awake()
        {
            m_Emoter = GetComponentInChildren<Emoter>();
        }

        public void HandleMoodManual(Consts.Moods.Mood mood)
        {
            Reset();
            
            // TODO: Maybe it'll be better to manually chose an emote rather than a random one from the mood pool.
            m_Emoter.ManualEmote(Consts.Moods.GetRandomEmoteFor(m_Emoter, mood).expData.name,
                ExpressionComponent.ExpressionHandler.OneWay);
        }

        public void Reset()
        {
            m_Emoter.useRandomEmotes = false;

            m_Emoter.TurnOffAll();

            foreach (var emoteExpression in m_Emoter.emotes)
            {
                emoteExpression.isLipsyncEmphasisEmote = false;
                emoteExpression.isRandomEmote = false;
                emoteExpression.isRepeaterEmote = false;
            }
            
            m_Emoter.UpdateEmoteLists();
        }

        public void HandleMoodRandom(Consts.Moods.MoodData mood)
        {
            Reset();
            
            m_Emoter.randomEmotes.Clear();
            
            foreach (var emoteExpression in Consts.Moods.GetMood(m_Emoter,mood.MMood))
            {
                emoteExpression.isRandomEmote = mood.IsRandom;
                emoteExpression.isLipsyncEmphasisEmote = mood.IsEmphasis;
                emoteExpression.isAlwaysEmphasisEmote = mood.IsEmphasis;
            }

            m_Emoter.randomEmotes.AddRange(Consts.Moods.GetMood(m_Emoter, mood.MMood).Where(e => e.isRandomEmote));
            m_Emoter.lipsyncEmphasisEmotes.AddRange(Consts.Moods.GetMood(m_Emoter, mood.MMood)
                .Where(e => e.isLipsyncEmphasisEmote));
            
            m_Emoter.useRandomEmotes = true;
            
            // m_Emoter.isChancePerEmote = false;
            //
            // m_Emoter.randomChance = 1f;
            
            m_Emoter.UpdateEmoteLists();
        }
    }
}