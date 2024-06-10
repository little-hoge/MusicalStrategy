using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Item : MonoBehaviour
{
    public Image FillImage;
    public SphereCollider attackCollider;
    public float damage, activeTimer;
    float PaintSpeed;
    public ParticleSystem attackEffect;
    public GameObject SE;
    void Start()
    {
        PaintSpeed = activeTimer;
    }
    //オブジェクト有効時
    void OnEnable()
    {
        StartCoroutine(PerformAttack());
    }
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

            // 敵であり、かつ味方でない場合にのみダメージを与える
            if (target != null && target.gameObject.tag != this.gameObject.tag)
            {
                target.ItemDamage(target, damage);
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