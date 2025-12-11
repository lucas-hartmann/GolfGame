using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip hitBallClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip buttonClickClip;

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object alive when changing scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate managers from other scenes
        }
    }

    // Call this to play simple Sound Effects
    public void PlaySFX(AudioClip clip)
    {
        // PlayOneShot allows multiple sounds to overlap (e.g., fast clicking)
        sfxSource.PlayOneShot(clip);
    }

    // Specific methods for your game events (Cleaner code elsewhere)
    public void PlayHitSound() => PlaySFX(hitBallClip);
    public void PlayWinSound() => PlaySFX(winClip);
    public void PlayLoseSound() => PlaySFX(loseClip);
    public void PlayClickSound() => PlaySFX(buttonClickClip);

    // Call this to change Background Music
    public void PlayMusic(AudioClip musicClip)
    {
        // Only switch if the requested music is different from what's playing
        if (musicSource.clip == musicClip) return;

        musicSource.Stop();
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}