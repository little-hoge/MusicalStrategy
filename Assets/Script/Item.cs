using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item : MonoBehaviour
{
    public Image FillImage;
    public SphereCollider attackCollider;
    public int Cost;
    public float damage, activeTimer;
    float PaintSpeed;
    public ParticleSystem attackEffect;
    public GameObject SE;
    TeamType Team;
    Color blueTeamColor, redTeamColor;
    TeamType MyTeam(GameObject obj) =>
      obj.CompareTag("RedTeam") ? TeamType.Red :
      obj.CompareTag("BlueTeam") ? TeamType.Blue :
      TeamType.None;

    void OnEnable()
    {
        PaintSpeed = activeTimer;
        FillImage = GameObject.Find("Fill").GetComponent<Image>();
        Debug.Log(FillImage);
        Team = MyTeam(gameObject);
        SetSliderColor();
        StartCoroutine(PerformAttack());
    }

    void SetSliderColor()
    {       
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");
        FillImage.color = Team == TeamType.Blue ? blueTeamColor : redTeamColor;
    }
    //オブジェクト有効時

    IEnumerator PerformAttack()
    {                       
        //タイマーが終わるまでは処理をしない
        while (activeTimer >= 0)
        {
            activeTimer -= Time.deltaTime;
            FillImage.fillAmount = activeTimer / PaintSpeed;
            yield return null;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackCollider.radius);
        foreach (var hitCollider in hitColliders)
        {
            Character target = hitCollider.GetComponent<Character>();
            if(target != null) 
            { 
                 TeamType enemyTeam = MyTeam(target.gameObject);
                // 敵であり、かつ味方でない場合にのみダメージを与える
                if (enemyTeam != Team)target.ItemDamage(target, damage);
            }
        }
        Instantiate(attackEffect, transform.position, Quaternion.identity);
        Instantiate(SE, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
    }
}