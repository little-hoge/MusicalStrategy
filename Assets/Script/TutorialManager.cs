using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

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
        if (tutorialFlowchart.GetBooleanVariable("Defeated")&& cost.cost <= 0) tutorialFlowchart.ExecuteBlock("NoCost");
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
        enemySummoningArea.AttemptSummon(SpawnPosition.transform.position, EnemyUnit[unitIndex], false);
        unitIndex += 1;
    }
    public void CharacterDefeated(Character character)
    {
        if(tutorialFlowchart.GetBooleanVariable("Defeated")) tutorialFlowchart.ExecuteBlock("NextTutorial");
        else tutorialFlowchart.ExecuteBlock("EnemyDefeated");
       /* if(character.tag == "RedTeam"&& character.isCastle) tutorialFlowchart.ExecuteBlock("CompleteDefeated");
        else if (character.tag == "Blue" && character.isCastle) tutorialFlowchart.ExecuteBlock("CastleDefeated");*/
    }

    public void EndTutorial()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(nextSceneIndex);
    }
}
