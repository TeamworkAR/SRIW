using System.Collections.Generic;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;

namespace UI.ClueCollection.ThoughtsCollection
{
    // TODO: Rename this to ThoughtsCollectionCharacterSelectionPanel
    // public class ThoughtsCollectionMainPanel : ThoughtsCollectionUI.ThoughtsCollectionPanel
    // {
    //     [SerializeField] private Transform m_CharactersLeftGroupContainer = null;
    //     
    //     [SerializeField] private Transform m_CharactersRightGroupContainer = null;
    //
    //     [SerializeField] private ThoughtsCollectionCharacterButton m_CharacterButtonTemplate = null;
    //
    //     private ScenarioSettings.ClueCollectionExtension m_Data = null;
    //
    //     private List<ThoughtsCollectionCharacterButton> m_Buttons = new List<ThoughtsCollectionCharacterButton>(0);
    //
    //     public override void Show()
    //     {
    //         base.Show();
    //         
    //         m_Data = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>();
    //         
    //         foreach (var rightGroupCharacter in m_Data.RightGroupCharacters)
    //         {
    //             ThoughtsCollectionCharacterButton button = Instantiate(m_CharacterButtonTemplate, m_CharactersRightGroupContainer);
    //             
    //             m_Buttons.Add(button);
    //             
    //             button.Show(rightGroupCharacter);
    //         }
    //         
    //         foreach (var leftGroupCharacter in m_Data.LeftGroupCharacters)
    //         {
    //             ThoughtsCollectionCharacterButton button = Instantiate(m_CharacterButtonTemplate, m_CharactersLeftGroupContainer);
    //             
    //             m_Buttons.Add(button);
    //             
    //             button.Show(leftGroupCharacter);
    //         }
    //     }
    //
    //     public override void Hide()
    //     {
    //         base.Hide();
    //
    //         m_Data = null;
    //         
    //         foreach (var thoughtsCollectionCharacterButton in m_Buttons)
    //         {
    //             Destroy(thoughtsCollectionCharacterButton.gameObject);   
    //         }
    //         m_Buttons.Clear();
    //     }
    // }
}