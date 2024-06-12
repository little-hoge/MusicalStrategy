using UnityEngine;
using TMPro;
public class Cost : MonoBehaviour
{
    TextMeshProUGUI tmp;
    [HideInInspector] public int cost = 0;
    float DefaultTime = 2, timer;
    void Start()
    {
        timer = DefaultTime;
        tmp = GetComponent<TextMeshProUGUI>();
        if (tmp != null) tmp.SetText(cost.ToString());
    }
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        UpdateCostUI(); 
        if (timer <= 0)
        {
            cost = Mathf.Min(cost + 1, 10); 
            timer = DefaultTime;
        }
    }
    // コストが変更されたらUIを更新
    public void UpdateCostUI()
    {
        if (tmp != null) tmp.SetText(cost.ToString());
    }
}