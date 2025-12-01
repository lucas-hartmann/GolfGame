using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ambientSource;
    public AudioSource sfxSource;

    public AudioClip ambientClip;
    public AudioClip golfHitClip;

    private float ambientVolume = 0.1f;
    private float sfxVolume = 1f;

    void Start()
    {
        ambientSource.volume = ambientVolume;
        sfxSource.volume = sfxVolume;

        // Start background sounds
        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void PlayGolfHit()
    {
        sfxSource.PlayOneShot(golfHitClip);
    }

}

