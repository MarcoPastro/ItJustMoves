using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] 
    private AudioSource _MusicSource;
    [SerializeField]
    private AudioSource _EffectsSource;
    public void PlaySound(AudioClip clip)
    {
        _EffectsSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)
    {
        _MusicSource.clip=clip;
        _MusicSource.Play();
    }
    public void StopMusic()
    {
        _MusicSource.Stop();
    }
}
