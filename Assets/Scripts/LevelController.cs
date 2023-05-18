using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField]
    private ScriptableId _IdMenuInputProvider;
    [SerializeField]
    private ScriptableId _IdGameplayInputProvider;
    [SerializeField]
    private PauseViewController _PauseMenu;

    [SerializeField]
    private AudioClip _LevelMusic;

    private void Awake()
    {
        PlayerController.Instance.EnableInputProvider(_IdGameplayInputProvider.Id);
        PlayerController.Instance.DisableInputProvider(_IdMenuInputProvider.Id);
    }
    private void OnEnable()
    {
        PlayMusic();
        PlayerController.Instance.OnActivePause += PauseMenu;
    }
    private void OnDisable()
    {
        StopMusic();
        PlayerController.Instance.OnActivePause -= PauseMenu;
    }
    public void PlayMusic()
    {
        AudioController.Instance.PlayMusic(_LevelMusic);
    }
    public void StopMusic()
    {
        AudioController.Instance.StopMusic();
    }
    public void PauseMenu(bool value)
    {
        if (value)
        {
            PlayerController.Instance.DisableInputProvider(_IdGameplayInputProvider.Id);
            PlayerController.Instance.EnableInputProvider(_IdMenuInputProvider.Id);
            _PauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PlayerController.Instance.EnableInputProvider(_IdGameplayInputProvider.Id);
            PlayerController.Instance.DisableInputProvider(_IdMenuInputProvider.Id);
            _PauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
