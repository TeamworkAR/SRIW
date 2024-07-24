using System;
using UI;
using UI.Smartphone;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ShowSmartPhoneUIAction : Action
    {
        [SerializeField] private SmartphoneChain m_Chain = null;
        
        public override void Execute()
        {
            MainGUI.Instance.MSmartphoneUI.Show(m_Chain);
        }
    }
}