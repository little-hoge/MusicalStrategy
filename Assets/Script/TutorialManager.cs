using UnityEngine;
using Fungus;

public class TutorialManager : MonoBehaviour
{
    Flowchart tutorialFlowchart; // FungusのFlowchart
    Character[] characters;         // Characterスクリプトを参照するフィールド
    public SummoningArea enemySummoningArea;
    public GameObject[] EnemyUnit;
    public Collider castleCollider;
    int unitIndex = 0;
    public GameObject SpawnPosition;
    Cost cost;
    void Start()
    {
        tutorialFlowchart = FindObjectOfType<Flowchart>();
        cost = FindObjectOfType<Cost>();
    }
    private void Update()
    {
        characters = FindObjectsOfType<Character>();
    }
    // キャラクターの動きを制御するメソッド
    public void SetCharacterMovement(bool canMove)
    {
        foreach (Character character in characters) character.TutorialStop = !canMove;
    }

    // チュートリアルの特定の段階でキャラクターを動かしたり止めたりする
    public void CharacterMovement()
    {
        if (tutorialFlowchart.GetBooleanVariable("Go")) SetCharacterMovement(true);
        else SetCharacterMovement(false);
    }

    // チュートリアル中に敵ユニットを1体召喚するメソッド
    public void SpawnTutorialEnemyUnit()
    {

        /* if (enemySummoningArea != null && EnemyUnit != null && unitIndex >= 0 && unitIndex < EnemyUnit.Length)
         {
             GameObject specificEnemyUnit = EnemyUnit[unitIndex];
             if (specificEnemyUnit != null)
             {
                 Vector3 spawnPosition = GetRandomPositionInSummoningArea();
                 enemySummoningArea.AttemptSummon(spawnPosition, specificEnemyUnit, false);
                 unitIndex += 1;
             }
         }
         else Debug.LogError("EnemyUnitのインデックスが範囲外です。");*/
        enemySummoningArea.AttemptSummon(SpawnPosition.transform.position, EnemyUnit[unitIndex], false);
        unitIndex += 1;
    }

   /* Vector3 GetRandomPositionInSummoningArea()
    {
        Bounds bounds = enemySummoningArea.GetComponent<Collider>().bounds;
        Vector3 spawnPosition;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = 1;
            float z = Random.Range(bounds.min.z, bounds.max.z);
            spawnPosition = new Vector3(x ,y ,z);
        }
        while (castleCollider.bounds.Contains(spawnPosition));

        return spawnPosition;
    }*/
    public void Defeated(Character character)
    {
        if(tutorialFlowchart.GetBooleanVariable("Defeated")) tutorialFlowchart.ExecuteBlock("NextTutorial");
        else tutorialFlowchart.ExecuteBlock("EnemyDefeated");
    }

    public void Nocost()
    { if(cost.cost <= 1) tutorialFlowchart.ExecuteBlock("NoCost"); }
}
