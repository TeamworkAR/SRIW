using System;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;

namespace UI.Smartphone
{
    [Serializable]
    public class SmartphoneChain
    {
        [SerializeReference, ShowSerializeReference] private List<SmartPhoneChainEntry> m_Entries = new List<SmartPhoneChainEntry>(0);

        public List<SmartPhoneChainEntry> Entries => m_Entries;
    }
}