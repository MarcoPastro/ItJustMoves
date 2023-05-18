using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>, ISystem
{
    [SerializeField] 
    private AudioSource _MusicSource;
    [SerializeField]
    private AudioSource _EffectsSource;

    [SerializeField]
    private int _Priority;
    public int Priority { get => _Priority; }

    public void PlaySound(AudioClip clip)
    {
        _EffectsSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)//TODO mettere un audio system che ha tutte le clip e si passa un id
    {
        _MusicSource.clip=clip;
        _MusicSource.Play();
    }
    public void StopMusic()
    {
        _MusicSource.Stop();
    }

    public void Setup()
    {
        SystemCoordinator.Instance.FinishSystemSetup(this);
    }
}
