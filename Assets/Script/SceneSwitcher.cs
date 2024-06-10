using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    void Update()
    {
        // マウスの左クリックを検出
        if (Input.GetMouseButtonDown(0)) LoadNextScene();

    }

    void LoadNextScene()
    {
        // 現在のシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 次のシーンのインデックスを計算
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(nextSceneIndex);
        // デバッグ用
        else Debug.Log("これ以上シーンはありません。");

    }
}
