using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    
    public AudioClip explosionSound;
    public AudioClip hitSound;
    public AudioClip specialHitSound;
    public AudioClip wowSound;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Preload audio clips to avoid first-play delay
        explosionSound.LoadAudioData();
        hitSound.LoadAudioData();
        specialHitSound.LoadAudioData();
        wowSound.LoadAudioData();
    }
    
    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosionSound, 1.0f);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound, 1.0f);
    }

    public void PlaySpecialHitSound()
    {
        audioSource.PlayOneShot(specialHitSound, 1.0f);
    }

    public void PlayWowSound()
    {
        audioSource.PlayOneShot(wowSound, 1.0f);
    }
}