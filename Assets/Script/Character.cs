using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
public enum TeamType { None, Red, Blue }    // チーム（タグで判断）
public enum AttackType { only, wide, ranged }   // 攻撃タイプ
public class Character : MonoBehaviour
{
    public int MAXHP, Power, Defense = 0, bgmPartIndex, Speed, Range, attackAngle, projectileSpeed;
    int HP;
    public float AttackDelay;
    float DelayTimer, waitTimer = 1f;
    public ParticleSystem attackEffect;
    public AttackType type;
    public GameObject projectilePrefab;     // 投擲物
    public bool isStationary, isCastle;
    [HideInInspector] public bool isAttacking, TutorialStop;
    SphereCollider RangeCollider;           // 索敵範囲
    TeamType Team;
    Rigidbody rb;
    Animator anim;
    [HideInInspector] public Slider slider;
    Image fillImage;
    Color blueTeamColor, redTeamColor;
    GameObject enemyCastle, myCastle;
    bool Detected;
    TeamType MyTeam(GameObject obj)
    {
        if (obj.CompareTag("RedTeam")) return TeamType.Red; // RedTeamの場合はRedを返す
        else if (obj.CompareTag("BlueTeam")) return TeamType.Blue; // BlueTeamの場合はBlueを返す
        else return TeamType.None; // 上記の条件に該当しない場合はNoneを返す
    }
    // チームによってスライダーの色を変える
    void SetSliderColor()
    {
        // Fill 部分の Image コンポーネントを取得
        Image fillImage = slider.fillRect.GetComponent<Image>();

        //カラーコードをカラーに変換（一度ifを挟む必要あり？）
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");

        // チームタグに応じて色を設定
        fillImage.color = Team == TeamType.Blue ? blueTeamColor : redTeamColor;
    }
    void Awake()
    {
        if (isCastle) isStationary = true;
        HP = MAXHP;
        RangeCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        Team = MyTeam(gameObject);
        if (!isCastle) rb = GetComponent<Rigidbody>();

        // キャラクターが召喚されたときにBGMパートをアクティブにする
        BGMController.instance.RegisterCharacter(this);
        DelayTimer = AttackDelay;
        slider.maxValue = MAXHP;
        slider.value = HP;
        SetSliderColor();

        // isCastle フラグが true のキャラクターのみを対象にする
        Character[] castleCharacters = FindObjectsOfType<Character>().Where(character => character.isCastle).ToArray();

        foreach (Character castleCharacter in castleCharacters)
        {
            if (castleCharacter.Team != Team) enemyCastle = castleCharacter.gameObject;
            else myCastle = castleCharacter.gameObject;
        }

    }
    void Update()
    {
        if (TutorialStop) return;       
        waitTimer -= Time.deltaTime;
        while (waitTimer > 0) return;
        if (!isStationary && enemyCastle != null && !isAttacking)
        {
            Vector3 direction = (enemyCastle.transform.position - transform.position).normalized;
            direction.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 3f);
            rb.MovePosition(transform.position + direction * Speed *Time.deltaTime);
        }
        if (Detected) DetectAndAttackEnemy();
    }
    //索敵範囲
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Untagged"))
        {
            Character chara = other.GetComponent<Character>();
            if (chara != null && chara.gameObject != gameObject)
            {
                TeamType enemyTeam = MyTeam(chara.gameObject);
                // 自チームではない場合
                if (enemyTeam != Team) Detected = true;
            }
        }
    }
    //敵察知時の動き
    void DetectAndAttackEnemy()
    {
        Transform localEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider collider in hitColliders) 
        {
            if (!collider.CompareTag("Untagged")&& collider.gameObject.tag != gameObject.tag)
            {
                localEnemy = collider.transform;
            }
        }
        if (localEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, localEnemy.position);
            // 攻撃範囲内にいるかをRaycastで判定
            RaycastHit hit;
            if (Physics.Raycast(transform.position, localEnemy.position - transform.position, out hit))
            {
                Character target = hit.collider.GetComponent<Character>();
                if (target == localEnemy.GetComponent<Character>())
                {
                    // 攻撃範囲内にいる場合
                    if (distance <= Range)
                    {
                        isAttacking = true;
                        AttackDelay -= Time.deltaTime;
                        if (AttackDelay <= 0)
                        {
                            AttackIfInRange(target);
                            AttackDelay = DelayTimer;
                        }
                        return;
                    }
                }
            }
            // 動かない場合は無視
            if (!isStationary)
            {
                // 攻撃範囲外にいる場合の処理
                Vector3 direction = (localEnemy.position - transform.position).normalized;
                direction.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.DORotateQuaternion(lookRotation, 3f);
            }
        }                 
        else
        {
            Detected = false;
            isAttacking = false;
        }
    }
    //敵察知時の動き
    void AttackIfInRange(Character target)
    {
        Debug.Log(gameObject.name + "_Attack");
        if (target == null) return;
        anim.SetTrigger("isAttack");
        // キャラクターの位置からY軸をずらす（仮）
        Vector3 effectPosition = target.transform.position + new Vector3(0, 2.5f, 0);
        Instantiate(attackEffect, effectPosition, Quaternion.identity);     
        // 攻撃タイプに応じた処理
        switch (type)
        {
            case AttackType.only:
                Damage(target, target.bgmPartIndex);
                break;
            case AttackType.wide:
                PerformWideAttack();
                break;
            case AttackType.ranged:
                PerformRangedAttack(target);
                break;
        }
        isAttacking = false;
    }
    // 眼の前の敵を複数体　角度+レンジで攻撃
    void PerformWideAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range);
        foreach (var hitCollider in hitColliders)
        {
            Character target = hitCollider.GetComponent<Character>();
            if (target != null && MyTeam(target.gameObject) != Team)
            {
                // キャラクターの前方方向と敵の方向との間の角度を計算
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                // 角度が指定された範囲内にある場合に攻撃
                float angle = Vector3.Angle(transform.forward, directionToTarget);
                if (angle <= attackAngle) Damage(target, target.bgmPartIndex);
            }
        }
    }
    //遠距離攻撃の場合
    void PerformRangedAttack(Character target)
    {
        if (projectilePrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.forward * 1.5f;
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.LookAt(target.transform);

            // DOTweenを使用して投擲物を敵に向かって移動させる
            projectile.transform.DOMove(target.transform.position, projectileSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                // ダメージ処理を実行
                Damage(target, target.bgmPartIndex);
                Destroy(projectile);
            });
        }
    }
    void Damage(Character target, int bgmPartIndex)
    {
        target.HP -= Mathf.Max(0, Power - target.Defense);
        target.slider.value = target.HP;
        if (target.HP <= 0)
        {
            // 城が破壊された場合の勝利処理
            if (target.isCastle == true) Debug.Log(Team + " Win");
            Destroy(target.gameObject);
            BGMController.instance.UnregisterCharacter(target);
        }
        // 自分が城でありHPが0になった場合の敗北処理
        if (this.HP <= 0 && isCastle == true) Debug.Log(Team + " Lose");
    }
    //アイテムでダメージを負った場合
    public void ItemDamage(Character target, float damage)
    {
        target.HP -= Mathf.Max(0, (int)damage - Defense);
        target.slider.value = HP;

        if (target.HP <= 0)
        {
            if (target.isCastle) Debug.Log(Team + " Win");

            Destroy(target.gameObject);
            BGMController.instance.SetBGMPartActive(target.bgmPartIndex, false);
        }

        if (this.HP <= 0 && isCastle) Debug.Log(Team + " Lose");
    }
    //コライダーの可視化
    void OnDrawGizmos()
    {
        // 現在の索敵範囲を可視化
        RangeCollider = GetComponent<SphereCollider>();
        Gizmos.color = Color.green;

        // スケールを考慮した半径の計算
        float scaledRadius = RangeCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position, scaledRadius);

        // 攻撃範囲を可視化
        Gizmos.color = Color.red;
        float scaledAttackRange = Range * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Gizmos.DrawWireSphere(transform.position, Range);

        // 攻撃角度+範囲を可視化
        if (type == AttackType.wide)
        {
            Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle, 0) * transform.forward * Range;
            Vector3 rightBoundary = Quaternion.Euler(0, attackAngle, 0) * transform.forward * Range;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }
    }
}