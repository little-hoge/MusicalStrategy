using System.Collections.Generic;
using UnityEngine;
public class Deck : MonoBehaviour
{
    public List<GameObject> cardPool, handCards;
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
        int randomIndex = Random.Range(0, cardPool.Count);
        GameObject drawnCard = Instantiate(cardPool[randomIndex], parentObject);
        handCards.Add(drawnCard);

    }
}
