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

    [SerializeField] private UnityEngine.Audio.AudioMixer myMixer;

    void Start()
    {
        // Apply saved volume settings immediately
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVol = PlayerPrefs.GetFloat("musicVolume");
            float sfxVol = PlayerPrefs.GetFloat("sfxVolume");

            myMixer.SetFloat("MusicVol", Mathf.Log10(musicVol) * 20);
            myMixer.SetFloat("SFXVol", Mathf.Log10(sfxVol) * 20);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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