using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;
using System;

public class Main : MonoBehaviour
{
    // タイマーの初期値を設定
    float startTimer = 3f, gameTimer = 120f;
    Character[] characters;
    public GameObject resultPanel, winImage, loseImage, countDownPanel, gameTimePanel;
    public TextMeshProUGUI startText, timerText;

    void Awake()
    {
        // テキストコンポーネントを取得
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

        // ゲームタイマー
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            var span = new TimeSpan(0, 0, (int)gameTimer);
            timerText.text = span.ToString(@"mm\:ss");
            await UniTask.Yield();
        }
        SetCharacterMovement(false);  // キャラクターの移動を停止
    }

    // 勝敗判定メソッド
    public void WinOrLose(bool defeatResult)
    {
        SetCharacterMovement(false);
        resultPanel.SetActive(true);
        if (defeatResult) winImage.SetActive(true);
        else loseImage.SetActive(true);
    }
}