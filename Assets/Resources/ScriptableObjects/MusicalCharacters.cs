using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MusicalCharacters : ScriptableObject
{
    public List<CharacterStates> States;
}

[System.Serializable]
public class CharacterStates
{
    public int MAXHP,Power,Defense,Speed,Range,attackAngle, projectileSpeed, CharaCost, BGMPartIndex;
    public float AttackDelay;
}