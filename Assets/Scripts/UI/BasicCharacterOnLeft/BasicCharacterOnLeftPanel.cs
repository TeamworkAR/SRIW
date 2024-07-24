using Animation;
using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BasicCharacterOnLeftPanel : BaseUICanvas
{
    [SerializeField] private RawImage characterImage = null;

    private bool basicCharacterEnded = false;

    public bool IsDone() => basicCharacterEnded;


    public void ShowText(BasicCharacterOnLeftDataWrapper wrapper)
    {
        basicCharacterEnded = false;

        base.Show();

        CharacterShowcase characterShowcase = wrapper.BasicCharacterOnLeft.Owner.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

        if (characterShowcase == null)
        {
            characterImage.enabled = false;
        }
        else
        {
            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();
            characterImage.texture = characterShowcase.ImageTexture;
        }
    }

    public override void Show()
    {
        base.Show();

        basicCharacterEnded = false;
    }

    public override void Hide()
    {
        basicCharacterEnded = true;

        base.Hide();
    }

    public void EndInfoPanel()
    {
        basicCharacterEnded = true;

        Hide();
    }
}
