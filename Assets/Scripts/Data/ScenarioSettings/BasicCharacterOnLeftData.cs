using Data.CharacterData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BasicCharacterOnLeftData
{
    [SerializeField] private CharacterData owner = null;

    public CharacterData Owner => owner;
}
