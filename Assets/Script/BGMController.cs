using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;

    public AudioSource[] bgmParts; // 曲パートのオーディオソース

    private List<Character> registeredCharacters = new List<Character>();
    
    TutorialManager tutorialManager;

    void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // 全ての曲パートを非アクティブに設定
        for (int i = 0; i < bgmParts.Length; i++) bgmParts[i].volume = 0f;
    }

    // キャラクターを登録するメソッド
    public void RegisterCharacter(Character character)
    {
        if (!registeredCharacters.Contains(character))
        {
            registeredCharacters.Add(character);
            SetBGMPartActive(character.bgmPartIndex, true);
            //Debug.Log("Registered character: " + character.name + ", BGM Part Index: " + character.bgmPartIndex);
        }
    }

    // キャラクターを削除するメソッド
    public void UnregisterCharacter(Character character)
    {
        if (registeredCharacters.Contains(character))
        {
            registeredCharacters.Remove(character);

            // 現在のリストに同じbgmPartIndexを持つキャラクターが存在するか確認
            bool isAnyRemaining = registeredCharacters.Any(c => c.bgmPartIndex == character.bgmPartIndex);

            // 該当するbgmPartIndexを持つキャラクターが他に存在しない場合、BGMパートを非アクティブにする
            if (!isAnyRemaining)
            {
                SetBGMPartActive(character.bgmPartIndex, false);
                if(tutorialManager != null) tutorialManager.Defeated(character);
            }
        }
    }

    // 再生＆停止
    public void SetBGMPartActive(int index, bool isActive)
    {
        bgmParts[index].volume = isActive ? 0.5f : 0f;
    }
}
