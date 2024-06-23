using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System;
using System.Linq;

public class Main : MonoBehaviour
{
    float startTimer = 3f, gameTimer = 120f;
    Character[] characters;
    public GameObject resultPanel, winImage, loseImage, DrawImage, countDownPanel, gameTimePanel;
    TextMeshProUGUI startText, timerText;
    [HideInInspector]public bool GameStop;

    void Awake()
    {
        GameStop = true;
        startText = countDownPanel.GetComponentInChildren<TextMeshProUGUI>();
        timerText = gameTimePanel.GetComponentInChildren<TextMeshProUGUI>();
        // GameManagerメソッドを非同期で開始
        GameManager().Forget();
    }

    void Update()
    {
        // Characterオブジェクトを全て取得
        characters = FindObjectsOfType<Character>();
    }

    // キャラクターの移動を制御
    public void SetCharacterMovement(bool canMove)
    {
        foreach (Character character in characters)character.MoveStop = !canMove;  
    }

    // ゲーム管理メソッド
    async UniTaskVoid GameManager()
    {
        // スタートカウントダウン
        while (startTimer >= 0)
        {
            startTimer -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(startTimer); // 小数点以下を切り上げ
            startText.text = seconds.ToString("0");    // 下1桁のみ表示
            await UniTask.Yield();  // タイマーのカウントダウンを待機
        }
        SetCharacterMovement(true);  // キャラクターの移動を開始
        countDownPanel.SetActive(false);
        GameStop = false;
        BGMController.instance.SetBGMPartActive(0, true);
        // ゲームタイマー
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            var span = new TimeSpan(0, 0, (int)gameTimer);
            timerText.text = span.ToString(@"mm\:ss");
            await UniTask.Yield();
        }
        SetCharacterMovement(false);  // キャラクターの移動を停止
        WinOrLose(0);
    }
    // 勝敗判定メソッド
    public void WinOrLose(int defeatResult)
    {
        GameStop = true;
        SetCharacterMovement(false);
        resultPanel.SetActive(true);

        // 勝利/敗北/引き分けの表示
        if (defeatResult == 1) winImage.SetActive(true);
        else if (defeatResult == 2) loseImage.SetActive(true);
        else
        {
            // 自チームの城と敵チームの城のHPを比較
            var redCastle = characters.FirstOrDefault(c => c.isCastle && c.Team == TeamType.Red);
            var blueCastle = characters.FirstOrDefault(c => c.isCastle && c.Team == TeamType.Blue);
            if (blueCastle.HP > redCastle.HP) winImage.SetActive(true);
            else if (blueCastle.HP < redCastle.HP) loseImage.SetActive(true);
            else DrawImage.SetActive(true);
        }
    }
}