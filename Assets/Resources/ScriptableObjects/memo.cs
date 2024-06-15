/*using UnityEngine;

CardEntity.cs =======

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
//カードデータそのもの
public class CardEntity : ScriptableObject
{
    public int cost;
}

==================
CardModel.cs=======

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードデータそのものとその処理
public class CardModel
{
    public int cost; //コスト
}
 public void SetCardModel(int cardID)
 {
     CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/" + cardID);
     cost = cardEntity.cost;
 }
==================
maingame.cs=======
void Start()
{
   CardModel card = CreateCard(cardid);
   Item item = objectToSummon.GetComponent<Item>();
   item.cost = card.cost;
}

//キャラの情報を取得
CardModel CreateCard(int cardid)
 {
     //カードの生成とデータの受け渡し
     CardModel card = Instantiate(cardPrefab, pos, Quaternion.identity);
  　 card.SetCardModel(cardid);
      return card;
 }
*/