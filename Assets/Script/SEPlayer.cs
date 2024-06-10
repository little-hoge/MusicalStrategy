using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    public AudioClip SE;

    void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SE;
        audioSource.Play();

        // サウンドが終わったらこのオブジェクトを削除
        Destroy(gameObject, SE.length);
    }
}
