using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject[] enemyDeck; // 敵側のデッキ
    int maxUnitsPerWave = 1; // 最大ユニット数
    private SummoningArea enemySummoningArea;
    private Cost enemyCost; // 敵のコスト管理
    Main main;
    void Awake()
    {
        main = FindObjectOfType<Main>();
        enemySummoningArea = GameObject.Find("SummonArea_Red").GetComponent<SummoningArea>();
        enemyCost = this.gameObject.GetComponent<Cost>();
    }
    void FixedUpdate()
    {
        if (!main.GameStop) SpawnEnemyUnits();
    }
    void SpawnEnemyUnits()
    {
        if (enemyDeck == null) return; // enemyDeck が null なら実行しない

        // 利用可能なユニットを取得し、ランダムにシャッフルしてから最大ユニット数分選択する
        var availableUnits = enemyDeck
            .Where(unit => unit != null && unit.GetComponent<CharaState>() != null)
            .Where(unit => unit.GetComponent<CharaState>().Cost <= enemyCost.cost)
            .OrderBy(unit => Random.value)
            .Take(maxUnitsPerWave)
            .ToList();

        int unitsToSpawn = Mathf.Min(availableUnits.Count, maxUnitsPerWave);
        for (int i = 0; i < unitsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableUnits.Count);
            GameObject unitToSpawn = availableUnits[randomIndex];
            CharaState chara = unitToSpawn.GetComponent<CharaState>();
            if (chara != null && enemyCost.cost >= chara.Cost)
            {
                //Debug.Log("enemyCost_" + enemyCost.cost + "charaCost" + chara.Cost);
                enemyCost.cost -= chara.Cost;
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