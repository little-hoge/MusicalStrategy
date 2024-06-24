using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
public class Item : MonoBehaviour
{
    public Image FillImage;
    SphereCollider attackCollider;
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
    void Start()
    {
        PaintSpeed = activeTimer;
        attackCollider = this.GetComponentInChildren<SphereCollider>();
        Team = MyTeam(gameObject);
        SetSliderColor();
        PerformAttack().Forget();
    }

    void SetSliderColor()
    {
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");
        FillImage.color = Team == TeamType.Blue ? redTeamColor : blueTeamColor;
    }
    async UniTaskVoid PerformAttack()
    {
        // タイマーが終了するまで処理する
        while (activeTimer >= 0)
        {
            activeTimer -= Time.deltaTime;
            FillImage.fillAmount = activeTimer / PaintSpeed;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        //オブジェクトの大小に合わせたスケーリング
        Vector3 scale = transform.lossyScale;
        float largestScale = Mathf.Max(scale.x, scale.y, scale.z);
        float scaledRadius = attackCollider.radius * largestScale;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scaledRadius)
                                       .Where(collider => collider is BoxCollider)
                                       .ToArray();
        foreach (var hitCollider in hitColliders)
        {
            Character target = hitCollider.GetComponent<Character>();
            if (target != null)
            {
                TeamType enemyTeam = MyTeam(target.gameObject);
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
        float largestScale = Mathf.Max(scale.x, scale.y, scale.z);
        float scaledRadius = attackCollider.radius * largestScale;
        Gizmos.DrawWireSphere(transform.position, scaledRadius);
    }
}