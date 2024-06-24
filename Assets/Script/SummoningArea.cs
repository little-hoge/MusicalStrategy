using UnityEngine;
using Fungus;

public class SummoningArea : MonoBehaviour
{
    // 召喚エリアコライダー
    new Collider collider;
    // マテリアルの設定
    public Material blueTeamMaterial, redTeamMaterial;
    public Flowchart tutorialFlowchart;
    void Start()
    {
        collider = this.GetComponent<Collider>();
        if (collider == null) Debug.LogError("SummoningAreaスクリプトがアタッチされているオブジェクトにColliderがありません。");
    }
    public GameObject AttemptSummon(Vector3 dropPosition, GameObject objectToSummon, bool isEnemy)
    {

        if (objectToSummon == null) Debug.LogError("召喚するオブジェクトが指定されていません。");

        // プレハブの回転角を設定
        Quaternion summonRotation = isEnemy ? Quaternion.Euler(transform.rotation.x, -90, 0) : Quaternion.Euler(transform.rotation.x, 90, 0);

        // プレハブを指定の位置と回転で生成
        GameObject summonedObject = Instantiate(objectToSummon, dropPosition, summonRotation);
        Character[] chara = summonedObject.GetComponentsInChildren<Character>();
        Item item = summonedObject.GetComponent<Item>();
        if (item != null)
        {
            item.gameObject.tag = isEnemy ? "BlueTeam" : "RedTeam";
            item.transform.rotation = Quaternion.identity;
        }
        foreach (Character charas in chara)
        {      
            // Renderer のマテリアルを設定
            Renderer unitRenderer = charas.GetComponentInChildren<Renderer>();
            charas.gameObject.tag = isEnemy ? "BlueTeam" : "RedTeam";
            unitRenderer.material = isEnemy ? blueTeamMaterial : redTeamMaterial;
        }

        if (tutorialFlowchart != null) tutorialFlowchart.ExecuteBlock("CardSummoned");
        // 生成されたオブジェクトを返す
        return summonedObject;
    }
}