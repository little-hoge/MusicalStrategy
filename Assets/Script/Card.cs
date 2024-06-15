using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform imageTransform, canvasRectTransform;
    Vector2 initialPosition;
    Canvas canvas;
    SummoningArea summoningArea;
    Collider summoningCollider;
    HorizontalLayoutGroup horizontalLayoutGroup;
    public GameObject objectToSummon;
    int CardCost;
    Cost cost;
    void Start()
    {
        CharaState character = objectToSummon.GetComponent<CharaState>();
        Item item = objectToSummon.GetComponent<Item>();
        if (character != null) CardCost = character.Cost;
        else if (item != null) CardCost = item.Cost;
        Debug.Log("Object to summon: " + objectToSummon + objectToSummon.gameObject + "_Card cost: " + CardCost);

        imageTransform = this.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();

        cost = canvas.GetComponentInChildren<Cost>();
        summoningArea = GameObject.Find("SummonArea_Blue").GetComponent<SummoningArea>();
        summoningCollider = summoningArea.GetComponent<Collider>();

        // 初期位置を保存
        initialPosition = imageTransform.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //HorizontalLayoutGroupを一時的に非表示
        if (horizontalLayoutGroup != null) horizontalLayoutGroup.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CardCost <= cost.cost)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                Vector3 globalPointerPosition = canvasRectTransform.TransformPoint(localPointerPosition);
                imageTransform.position = globalPointerPosition;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (CardCost <= cost.cost)
        {
            Vector3 worldDropPosition;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out worldDropPosition))
            {
                if (IsInsideSummoningArea(worldDropPosition))
                {
                    GameObject summonedCharacter = summoningArea.AttemptSummon(worldDropPosition, objectToSummon, true);
                    Destroy(gameObject);
                    cost.cost -= CardCost;
                    cost.UpdateCostUI();
                }
                else imageTransform.anchoredPosition = initialPosition;
            }
            horizontalLayoutGroup.enabled = true;
        }
    }
    private bool IsInsideSummoningArea(Vector3 worldPosition)
    {
        worldPosition.y = summoningCollider.bounds.center.y;
        Bounds bounds = summoningCollider.bounds;
        return bounds.Contains(worldPosition);
    }
}