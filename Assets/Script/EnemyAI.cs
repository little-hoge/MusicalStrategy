using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Card Card;
    public List<GameObject> enemyDeck;  // 敵側のデッキ
    public int maxUnitsPerWave = 3;     // 1回のウェーブで生成する最大ユニット数
    private SummoningArea enemySummoningArea;
    private Cost enemyCost;             // 敵のコスト管理

    void Awake()
    {
        enemySummoningArea = FindObjectOfType<SummoningArea>();
        enemyCost = this.gameObject.GetComponent<Cost>();
    }

    void FixedUpdate()
    {
        if (enemyCost.cost<=3) SpawnEnemyUnits();
    }

    void SpawnEnemyUnits()
    {
        int unitsToSpawn = Random.Range(1, maxUnitsPerWave + 1);
        var availableUnits = enemyDeck.Where(unit => enemyCost.cost >= 1).ToList();

        if (availableUnits.Count == 0) return;

        for (int i = 0; i < unitsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableUnits.Count);
            GameObject unitToSpawn = availableUnits[randomIndex];

            // コストを消費
            enemyCost.cost -= 3;

            // 召喚位置をランダムに決定
            Vector3 spawnPosition = GetRandomPositionInSummoningArea();

            // ユニットを召喚
            enemySummoningArea.AttemptSummon(spawnPosition, unitToSpawn, false);
        }
    }

    Vector3 GetRandomPositionInSummoningArea()
    {
        Bounds bounds = enemySummoningArea.GetComponent<Collider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = 0;
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }
}
