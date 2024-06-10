using UnityEngine;
using Fungus;

public class SummoningArea : MonoBehaviour
{
    // 召喚エリアコライダー
    new Collider collider;
    // マテリアルの設定
    public Material blueTeamMaterial;
    public Material redTeamMaterial;

    Color blueTeamColor;
    Color redTeamColor;

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

        //スライダー用調整カラー
        if (!ColorUtility.TryParseHtmlString("#7982FF", out blueTeamColor)) Debug.LogError("ブルーチームのカラーの解析に失敗しました");
        if (!ColorUtility.TryParseHtmlString("#FF727A", out redTeamColor)) Debug.LogError("レッドチームのカラーの解析に失敗しました");


        //スライダーカラー/アイテムのチーム設定
        Color teamColor = isEnemy ? blueTeamColor : redTeamColor;
        if (item != null)
        {
            item.FillImage.color = teamColor;
            item.gameObject.tag = isEnemy ? "BlueTeam" : "RedTeam";
        }

        //isEnemy ? = if(isEnemy != true):else
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
