using UnityEngine;

public class LevelMusicSetter : MonoBehaviour
{
    public AudioClip levelMusic;

    void Start()
    {
        if (SoundManager.Instance != null && levelMusic != null)
        {
            SoundManager.Instance.PlayMusic(levelMusic);
        }
    }
}