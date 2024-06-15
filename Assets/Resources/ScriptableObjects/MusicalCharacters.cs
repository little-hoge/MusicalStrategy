using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MusicalCharacters : ScriptableObject
{
    public List<StatesEntity> States;
}

[System.Serializable]
public class StatesEntity
{
    public int MAXHP,Power,Defense,Speed,Range,attackAngle, projectileSpeed, CharaCost, BGMPartIndex;
    public float AttackDelay;
}