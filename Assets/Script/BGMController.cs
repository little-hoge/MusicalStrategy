using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BGMController : MonoBehaviour
{
    public static BGMController instance;
    public AudioSource[] bgmParts; // 曲パートのオーディオソース
    private List<Character> registeredCharacters = new List<Character>();    
    TutorialManager tutorialManager;
    Main main;
    void Awake()
    {
        main = FindObjectOfType<Main>();
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
            SetBGMPartActive(character.state.BGMPartIndex, true);
        }
    }
    // キャラクターを削除するメソッド
    public void UnregisterCharacter(Character character)
    {
        if (registeredCharacters.Contains(character))
        {
            registeredCharacters.Remove(character);

            // 現在のリストに同じbgmPartIndexを持つキャラクターが存在するか確認
            bool isAnyRemaining = registeredCharacters.Any(c => c.state.BGMPartIndex == character.state.BGMPartIndex);

            // 該当するbgmPartIndexを持つキャラクターが他に存在しない場合、BGMパートを非アクティブにする
            if (!isAnyRemaining)
            {
                SetBGMPartActive(character.state.BGMPartIndex, false);
                if(tutorialManager != null) tutorialManager.CharacterDefeated(character);
            }
        }
    }
    // 再生＆停止
    public void SetBGMPartActive(int index, bool isActive)
    {
       if(!main.GameStop) bgmParts[index].volume = isActive ? 0.5f : 0f;
    }
}