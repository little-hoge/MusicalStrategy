using UnityEngine;

//テスト用読み込みスクリプト
public class CharaState : MonoBehaviour
{
    public int ID;   
    [HideInInspector]public int MAXHP, Power, Defense, Speed, Range, attackAngle, projectileSpeed, CharaCost,BGMPartIndex;
    [HideInInspector] public float AttackDelay;
    void Awake()
    {
        LoadCharaData();
    }

    public void LoadCharaData()
    {
        MusicalCharacters charaData = Resources.Load<MusicalCharacters>("ScriptableObjects/MusicalCharacters");
        if (charaData == null)
        {
            Debug.LogError("MusicalCharacters ScriptableObject が見つかりませんでした。");
            return;
        }

        MAXHP = charaData.States[ID].MAXHP;
        Power = charaData.States[ID].Power;
        Defense = charaData.States[ID].Defense;
        Speed = charaData.States[ID].Speed;
        Range = charaData.States[ID].Range;
        attackAngle = charaData.States[ID].attackAngle;
        projectileSpeed = charaData.States[ID].projectileSpeed;
        CharaCost = charaData.States[ID].CharaCost;
        BGMPartIndex = charaData.States[ID].BGMPartIndex;
        AttackDelay = charaData.States[ID].AttackDelay;
    }
}