using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyAI : MonoBehaviour
{
    public GameObject[] enemyDeck; // 敵側のデッキ
    private int maxUnitsPerWave = 3; // 最大ユニット数
    private SummoningArea enemySummoningArea;
    private Cost enemyCost; // 敵のコスト管理
    private Main main;

    void OnEnable()
    {
        main = FindObjectOfType<Main>();
        enemySummoningArea = GameObject.Find("SummonArea_Red").GetComponent<SummoningArea>();
        enemyCost = GetComponent<Cost>();
        StartSpawningEnemies().Forget(); // 非同期処理を開始
    }

    async UniTaskVoid StartSpawningEnemies()
    {
        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

            if (main.GameStop) continue;

            float randomDelay = Random.Range(0, 5f); // ランダムな秒数を取得
            while (randomDelay > 0)
            {
                randomDelay -= Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update); // フレームの終了を待つ
            }

            if (enemyCost.cost > 0) // コストがある場合に召喚を試みる
            {
                // 利用可能なユニットを取得し、ランダムにシャッフルしてから最大ユニット数分選択する
                var availableUnits = enemyDeck
                    .Where(unit => unit != null && unit.GetComponent<CharaState>() != null)
                    .Where(unit => unit.GetComponent<CharaState>().Cost <= enemyCost.cost)
                    .OrderBy(unit => Random.value)
                    .Take(Random.Range(1, 4)) // 1～3体を選択
                    .ToList();

                if (availableUnits.Count > 0)
                {
                    Debug.Log($"Available units: {availableUnits.Count}");

                    int unitsToSpawn = Mathf.Min(availableUnits.Count, Random.Range(1, 4)); // 1～3体を選択
                    for (int i = 0; i < unitsToSpawn; i++)
                    {
                        int randomIndex = Random.Range(0, availableUnits.Count);
                        GameObject unitToSpawn = availableUnits[randomIndex];
                        CharaState chara = unitToSpawn.GetComponent<CharaState>();
                        if (chara != null && enemyCost.cost >= chara.Cost)
                        {
                            enemyCost.cost -= chara.Cost;
                            Vector3 spawnPosition = GetRandomPositionInSummoningArea();
                            enemySummoningArea.AttemptSummon(spawnPosition, unitToSpawn, false);
                            Debug.Log($"Spawned unit: {unitToSpawn.name}");
                            availableUnits.RemoveAt(randomIndex); // 召喚したユニットをリストから削除
                        }
                    }
                }
                else Debug.Log("No available units to spawn.");
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
