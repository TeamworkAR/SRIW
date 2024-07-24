using System;
using System.Collections.Generic;
using CrazyMinnow.SALSA;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public static class Consts
    {
        public static class UI
        {
            public const float k_BACK_BUTTON_CD = 2f;

            public const float k_DECISION_MAKING_END_CD = 4f;
            
            public const float k_BUTTON_FADE_DURATION = 0.1f;
        }

        public static class Animation
        {
            public const string k_TRIGGER_CHARACTERSHOWCASE_RESET = "Reset";

            public const string k_TRIGGER_CHARACTERSHOWCASE_MOODCHECKIN = "MoodCheckIn";
            
            public const string k_BOOL_CHARACTERSHOWCASE_MOODCHECKIN_ISSELECTED = "MoodCheckIn_Selected";

            public const string k_TRIGGER_CHARACTERSHOWCASE_CONTEXTSETTINGS = "ContextSettings";

            public const string k_TRIGGER_CHARACTERSHOWCASE_THOUGHTCOLLECTION = "ThoughtCollection";

            public const string k_TRIGGER_CHARACTERSHOWCASE_POLICYCOLLECTION = "PolicyCollection";

            public const string k_TRIGGER_CHARACTERSHOWCASE_DECISIONMAKING = "DecisionMaking";

            public const string k_TRIGGER_CHARACTERSHOWCASE_LEARNINGS = "Learnings";

            public const string k_TRIGGER_CHARACTERSHOWCASE_END = "End";

            public const string k_BOOL_DIALOGUE_ANIMATE = "IsActive";

            public const string k_STATE_DIALOGUE_ACTIVE = "Active";
            
            public const string k_STATE_DIALOGUE_INACTIVE = "Inactive";

            public const string k_CONTEXTSETTINGSIDLEOFFSET = "ContextSettingsIdleOffset";

            public const string k_THOUGHTSCOLLECTIONOFFSET = "ThoughtsCollectionOffset";
        }
    
        public static class Emotes
        {
            public const string k_EMOTE_SMILING = "smiling";
            public const string k_EMOTE_SURPRISED = "surprised";
            public const string k_EMOTE_SAD = "sad";
            public const string k_EMOTE_ANGRY = "angry";
            public const string k_EMOTE_NEUTRAL = "neutral";

            public const string k_EMOTE_ANNOYED = "annoyed";
            public const string k_EMOTE_CONFUSED = "confused";
            public const string k_EMOTE_UNTROUBLED = "untroubled";
            public const string k_EMOTE_HAPPY = "happy";
            public const string k_EMOTE_CHEERFUL = "cheerful";
            public const string k_EMOTE_WORRIED = "worried";
        }
    
        public static class Moods
        {
            public enum Mood
            {
                None = 0,
                Smiling = 1,
                Surprised = 2,
                Sad = 3,
                Angry = 4,
                Neutral = 5,

                Annoyed = 6,
                Confused = 7,
                Untroubled = 8,
                Happy = 9,
                Cheerful = 10,
                Worried = 11
            }

            private static Dictionary<Mood, List<string>> m_Moods = new Dictionary<Mood, List<string>>()
            {
                { Mood.None, new List<string>(0)},
                { Mood.Angry , new List<string>(){Consts.Emotes.k_EMOTE_ANGRY} },
                { Mood.Smiling , new List<string>(){Consts.Emotes.k_EMOTE_SMILING} },
                { Mood.Surprised , new List<string>(){Consts.Emotes.k_EMOTE_SURPRISED} },
                { Mood.Sad , new List<string>(){Consts.Emotes.k_EMOTE_SAD} },
                { Mood.Neutral , new List<string>(){Consts.Emotes.k_EMOTE_NEUTRAL} },
                { Mood.Annoyed , new List<string>(){Consts.Emotes.k_EMOTE_ANNOYED} },
                { Mood.Confused , new List<string>(){Consts.Emotes.k_EMOTE_CONFUSED} },
                { Mood.Untroubled , new List<string>(){Consts.Emotes.k_EMOTE_UNTROUBLED} },
                { Mood.Happy , new List<string>(){Consts.Emotes.k_EMOTE_HAPPY} },
                { Mood.Cheerful , new List<string>(){Consts.Emotes.k_EMOTE_CHEERFUL} },
                { Mood.Worried , new List<string>(){Consts.Emotes.k_EMOTE_WORRIED} }
            };
            
            public static List<EmoteExpression> GetMood(Emoter emoter, Mood mood)
            {
                List<EmoteExpression> moodExpression = new List<EmoteExpression>(0);
                
                foreach (var expression in m_Moods[mood])
                {
                    foreach (var emoterExpression in emoter.emotes)
                    {
                        if (string.CompareOrdinal(emoterExpression.expData.name, expression) == 0)
                        {
                            moodExpression.Add(emoterExpression);
                        }
                    }    
                }

                return moodExpression;
            }

            public static EmoteExpression GetRandomEmoteFor(Emoter emoter, Mood mood)
            {
                List<EmoteExpression> emotes = GetMood(emoter, mood);

                return emotes[Random.Range(0, emotes.Count)];
            }
            
            [Serializable]
            public class MoodData
            {
                [SerializeField] private Mood m_Mood = Mood.None;

                [SerializeField] private bool b_IsRandom = false;

                [SerializeField] private bool b_IsEmphasis = false;

                [SerializeField] private bool b_IsManual = false;

                public Mood MMood => m_Mood;

                public bool IsRandom => b_IsRandom;

                public bool IsEmphasis => b_IsEmphasis;

                public bool IsManual => b_IsManual;
            }
        }

        public static class EditorStrings
        {
            public const string k_EDITOR_HEADERS_UIREFERENCES = "UI References";
            
            public const string k_EDITOR_HEADERS_LOCALIZATION = "Localization";
            
            public const string k_EDITOR_HEADERS_TEMPLATES = "Templates";
        }

        public static class ScormKeys 
        {
            public const string k_LOCALIZATION_SERIALIZATION_KEY = "SelectedLocale";
        }

        public static class Stub
        {
            public const string k_LOC_STUB_TEXT_TITLE = "Lorem ipsum";
            
            public const string k_LOC_STUB_TEXT_LONG =
                "Lorem ipsum dolor sit amet, consectetur adipisci elit, sed do eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullamco laboriosam, nisi ut aliquid ex ea commodi consequatur. Duis aute irure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        }
    }
}