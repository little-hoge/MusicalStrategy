using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Cysharp.Threading.Tasks;

public enum TeamType { None, Red, Blue }
public enum AttackType { only, wide, ranged }

public class Character : MonoBehaviour
{
    public CharaState state;
    public int Cost;
    int HP;
    public AttackType type;
    public ParticleSystem attackEffect;
    public GameObject ProjectilePrefab;

    float WaitTimer = 1f,DelayTimer;
    public bool isStationary, isCastle;
    [HideInInspector] public bool TutorialStop;
    SphereCollider  RangeCollider;
    TeamType Team;
    Rigidbody rb;
    Animator anim;
    Slider slider;
    Image fillImage;
    Color blueTeamColor, redTeamColor;
    [HideInInspector] public GameObject enemyCastle;
    bool isAttacking, Detected;
    TutorialManager Tutorial;
    TeamType MyTeam(GameObject obj) =>
        obj.CompareTag("RedTeam") ? TeamType.Red :
        obj.CompareTag("BlueTeam") ? TeamType.Blue :
        TeamType.None;
    void SetSliderColor()
    {
        fillImage = slider.fillRect.GetComponent<Image>();
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");
     fillImage.color = Team == TeamType.Blue ? blueTeamColor : redTeamColor;
    }
    public void Awake()
    {
        state = this.gameObject.GetComponent<CharaState>();
        if (isCastle) isStationary = true;
        RangeCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>(); 
    }
    void Start()
    {
        if (!isCastle) rb = GetComponent<Rigidbody>();
        BGMController.instance.RegisterCharacter(this);
        slider = GetComponentInChildren<Slider>();
        Tutorial = FindObjectOfType<TutorialManager>();
        HP = state.MAXHP;
        DelayTimer = state.AttackDelay;
        Team = MyTeam(gameObject);
        slider.value = HP;
        SetSliderColor();
        var castleCharacters = FindObjectsOfType<Character>().Where(character => character.isCastle);
        foreach (var castleCharacter in castleCharacters)
        {
            if (castleCharacter.Team != Team) enemyCastle = castleCharacter.gameObject;
        }
    }

    void Update()
    {
        if (TutorialStop) return;
        WaitTimer -= Time.deltaTime;
        while (WaitTimer > 0) return;
        if (!isStationary && enemyCastle != null && !isAttacking)
        {
            Vector3 direction = (enemyCastle.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 3f);
            rb.MovePosition(transform.position + direction * state.Speed * Time.deltaTime);
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
    void DetectAndAttackEnemy()
    {
        var localEnemy = Physics.OverlapSphere(transform.position, state.Range)
                            .Select(collider => collider.GetComponent<Character>())
                            .FirstOrDefault(chara => chara != null && MyTeam(chara.gameObject) != Team);

        if (localEnemy != null)
        {
            var distance = Vector3.Distance(transform.position, localEnemy.transform.position);
            if (distance <= state.Range)
            {
                isAttacking = true;
                state.AttackDelay -= Time.deltaTime;
                if (state.AttackDelay <= 0)
                {
                    AttackIfInRange(localEnemy);
                    state.AttackDelay = DelayTimer;
                }
            }
            else if (!isStationary)
            {
                var direction = (localEnemy.transform.position - transform.position).normalized;
                direction.y = 0;
                transform.DORotateQuaternion(Quaternion.LookRotation(direction), 3f);
            }
        }
        else
        {
            Detected = false;
            isAttacking = false;
        }
    }
    void AttackIfInRange(Character target)
    {
        if (target == null) return;
        anim.SetTrigger("isAttack");
        var effectPosition = target.transform.position + new Vector3(0, 2.5f, 0);
        Instantiate(attackEffect, effectPosition, Quaternion.identity);
        switch (type)
        {
            case AttackType.only:
                Damage(target);
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
    void PerformWideAttack()
    {
        var hitColliders = Physics.OverlapSphere(transform.position,  state.Range);
        foreach (var hitCollider in hitColliders)
        {
            var target = hitCollider.GetComponent<Character>();
            if (target != null && MyTeam(target.gameObject) != Team)
            {
                var directionToTarget = (target.transform.position - transform.position).normalized;
                var angle = Vector3.Angle(transform.forward, directionToTarget);
                if (angle <=  state.attackAngle) Damage(target);
            }
        }
    }
    void PerformRangedAttack(Character target)
    {
        var spawnPosition = transform.position + Vector3.forward * 1.5f;
        var projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.LookAt(target.transform);
        projectile.transform.DOMove(target.transform.position, state.projectileSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            Damage(target);
            Destroy(projectile);
        });
    }  
    void Damage(Character target)
    {
        target.HP -= Mathf.Max(0, state.Power - target.state.Defense);
        target.slider.value = target.HP;
        if (target.HP <= 0)
        {
            if (target.isCastle)
            {
                if (Tutorial != null) Tutorial.CharacterDefeated(target);
                Debug.Log(Team + " Win");
            }
            Destroy(target.gameObject);
            BGMController.instance.UnregisterCharacter(target);

        }
        if (HP <= 0 && isCastle)
        {
            if (Tutorial != null) Tutorial.CharacterDefeated(target);
            Debug.Log(Team + " Lose");
        }
    }
    public void ItemDamage(Character target, float damage)
    {
        target.HP -= Mathf.Max(0, (int)damage - state.Defense);
        target.slider.value = HP;
        if (target.HP <= 0)
        {
            if (target.isCastle) Debug.Log(Team + " Win");
            Destroy(target.gameObject);
            BGMController.instance.UnregisterCharacter(target);
        }
        if (HP <= 0 && isCastle) Debug.Log(Team + " Lose");
    }
    void OnDrawGizmos()
    {
        if (RangeCollider != null)
        {
            Gizmos.color = Color.green;
            float scaledRadius = RangeCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            Gizmos.DrawWireSphere(transform.position, scaledRadius);
        }
        if(state.Range > 0)
        { 
            Gizmos.color = Color.red;
            float scaledAttackRange =  state.Range * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            Gizmos.DrawWireSphere(transform.position,  state.Range);
        }
        if (type == AttackType.wide)
        {
            Vector3 leftBoundary = Quaternion.Euler(0, - state.attackAngle, 0) * transform.forward *  state.Range;
            Vector3 rightBoundary = Quaternion.Euler(0,  state.attackAngle, 0) * transform.forward *  state.Range;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }
    }
}