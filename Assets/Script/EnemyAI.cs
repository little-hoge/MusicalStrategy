using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    CardList enemyDeck;  // 敵側のデッキ
    int maxUnitsPerWave = 1;     // 1回のウェーブで生成する最大ユニット数
    private SummoningArea enemySummoningArea;
    private Cost enemyCost;             // 敵のコスト管理
    void Awake()
    {
        enemyDeck = Resources.Load<CardList>("ScriptableObjects/CardList");
        if (enemyDeck == null)
        {
            Debug.LogError("CardList asset could not be found at path: ScriptableObjects/CardList");
            return;
        }
        enemySummoningArea = GameObject.Find("SummonArea_Red").GetComponent<SummoningArea>();
        enemyCost = GetComponent<Cost>();
    }
    void FixedUpdate()
    {
        SpawnEnemyUnits();
    }
    void SpawnEnemyUnits()
    {
        if (enemyDeck == null) return; // enemyDeck が null なら実行しない

        var availableUnits = enemyDeck.Card.Where(unit => unit.GetComponent<Character>().state.CharaCost <= enemyCost.cost).ToList();
        if (availableUnits.Count == 0) return;

        int unitsToSpawn = Mathf.Min(availableUnits.Count, maxUnitsPerWave);
        for (int i = 0; i < unitsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableUnits.Count);
            GameObject unitToSpawn = availableUnits[randomIndex];
            Character chara = unitToSpawn.GetComponent<Character>();
            if (chara != null && enemyCost.cost >= chara.state.CharaCost)
            {
                enemyCost.cost -= chara.state.CharaCost;               
                Vector3 spawnPosition = GetRandomPositionInSummoningArea();
                enemySummoningArea.AttemptSummon(spawnPosition, unitToSpawn, false);
                // 召喚したユニットをリストから削除
                availableUnits.RemoveAt(randomIndex);
            }
        }
    }
    // 召喚位置をランダムに決定
    Vector3 GetRandomPositionInSummoningArea()
    {
        Bounds bounds = enemySummoningArea.GetComponent<Collider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, 0, z);
    }
}