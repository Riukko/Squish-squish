using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip jumpSound;
    public AudioClip bgMusic;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Start()
    {
        musicSource.loop = true;
    }

    public void PlayJumpSound()
    {
        sfxSource.PlayOneShot(jumpSound);
    }

    public void PlayBgMusic()
    {
        musicSource.Play();
        
    }
}
