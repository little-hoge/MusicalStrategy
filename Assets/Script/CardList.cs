using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardList", menuName = "ScriptableObjects/CardList", order = 1)]
public class CardList : ScriptableObject
{
    public List<GameObject> Card;
}