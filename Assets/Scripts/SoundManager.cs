using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip coinClip;
    

    public void CoinSound()
    {
        audioSource.PlayOneShot(coinClip);
    }
}
