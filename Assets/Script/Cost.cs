using UnityEngine;
using TMPro;
public class Cost : MonoBehaviour
{
    TextMeshProUGUI tmp;
    [HideInInspector] public int cost;
    float DefaultTime = 2, timer;
    Main main;
    void Awake()
    {
        main = FindObjectOfType<Main>();
        timer = DefaultTime;
        tmp = GetComponent<TextMeshProUGUI>();
        cost = 5;
        if (tmp != null) tmp.SetText(cost.ToString());
    }
    void FixedUpdate()
    {
        if(!main.GameStop)
        { 
            timer -= Time.deltaTime;
            UpdateCostUI(); 
            if (timer <= 0)
            {
                cost = Mathf.Min(cost + 1, 10); 
                timer = DefaultTime;
            }
        }
    }
    // コストが変更されたらUIを更新
    public void UpdateCostUI()
    {
        if (tmp != null) tmp.SetText(cost.ToString());
    }
}