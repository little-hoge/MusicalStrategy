using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;
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
        if (FillImage == null) Debug.LogError("FillImageが見つかりませんでした。");
        Team = MyTeam(gameObject);
    }
    void Start()
    {
        SetSliderColor();
        PerformAttack().Forget();
    }

    void SetSliderColor()
    {
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");
        FillImage.color = Team == TeamType.Blue ? redTeamColor : blueTeamColor;
    }
    //オブジェクト有効時

    async UniTaskVoid PerformAttack()
    {
        // タイマーが終了するまで処理をしない
        while (activeTimer >= 0)
        {
            activeTimer -= Time.deltaTime;
            FillImage.fillAmount = activeTimer / PaintSpeed;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        Vector3 scale = transform.lossyScale;
        float largestScale = Mathf.Max(scale.x, scale.y, scale.z);
        float scaledRadius = attackCollider.radius * largestScale;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scaledRadius)
                                       .Where(collider => collider is BoxCollider)
                                       .ToArray();
        Debug.Log(scaledRadius);
        Debug.Log($"PerformAttack: Number of colliders detected: {hitColliders.Length}");

        foreach (var hitCollider in hitColliders)
        {
            Character target = hitCollider.GetComponent<Character>();
            if (target != null)
            {
                TeamType enemyTeam = MyTeam(target.gameObject);
                // 敵であり、かつ味方でない場合にのみダメージを与える
                if (enemyTeam != Team) target.ItemDamage(target, damage);
            }
        }

        Instantiate(attackEffect, transform.position, Quaternion.identity);
        Instantiate(SE, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 scale = transform.lossyScale;
        // 最大のスケールを計算
        float largestScale = Mathf.Max(scale.x, scale.y, scale.z);
        // 攻撃範囲のスケールを適用した半径を計算
        float scaledRadius = attackCollider.radius * largestScale;
        Gizmos.DrawWireSphere(transform.position, scaledRadius);
    }
}