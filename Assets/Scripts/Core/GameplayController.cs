using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private AudioClip _SusMusic;
    private void Awake()
    {
        AudioController.Instance.PlayMusic(_SusMusic);
    }
}
