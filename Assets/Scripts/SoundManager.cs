using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip coinClip;
    public AudioClip shotClip;
    public AudioClip confettiClip;
    public AudioClip clapClip;

    public void CoinSound()
    {
        audioSource.PlayOneShot(coinClip);
    }

    public void ShotSound()
    {
        audioSource.PlayOneShot(shotClip);
    }

    public void GoalSound()
    {
        audioSource.PlayOneShot(confettiClip);
        audioSource.PlayOneShot(clapClip);
    }
}
