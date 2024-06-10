using System.Collections.Generic;
using UnityEngine;
public class Deck : MonoBehaviour
{
    public List<GameObject> cardPool, handCards, tutorialDeck;
    public Transform parentObject;
    public bool tutorialMode;
    void Start()
    { 
        if (tutorialMode) foreach (var card in tutorialDeck) TutorialDraw(card);
        else for (int i = 0; i < 5; i++) DrawCard();
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
    public void TutorialDraw(GameObject card)
    {
        GameObject drawnCard = Instantiate(card, parentObject);
        handCards.Add(drawnCard);
    }
}
