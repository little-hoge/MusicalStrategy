using System.Collections.Generic;
using UnityEngine;
public class Deck : MonoBehaviour
{
    public List<GameObject> MyDeck, handCards;
    public Transform parentObject;
    public bool tutorialMode;
    void Start()
    { 
        for (int i = 0; i < 5; i++) DrawCard();
    }
    void Update()
    {
        if (!tutorialMode) 
        {
            // 手札が5枚以下の場合にカードを引く
            if (handCards.Count < 5) DrawCard();
            handCards.RemoveAll(card => card == null);
        }

    }
    public void DrawCard()
    {
        int randomIndex = Random.Range(0, MyDeck.Count);
        GameObject drawnCard = Instantiate(MyDeck[randomIndex], parentObject);
        handCards.Add(drawnCard);

    }
}