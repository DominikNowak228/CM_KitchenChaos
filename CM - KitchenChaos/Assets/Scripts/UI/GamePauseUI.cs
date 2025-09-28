using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Start() {
        GameManager.Instance.OnGamePaused += (sender, e) => { gameObject.SetActive(true); };
        GameManager.Instance.OnGameUnpaused += (sender, e) => { gameObject.SetActive(false); };

        resumeButton.onClick.AddListener(() => GameManager.Instance.TogglePauseGame());
        mainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainManuScene));
        optionsButton.onClick.AddListener(() => OptionsUI.Instance.gameObject.SetActive(true));

        gameObject.SetActive(false);
    }
}
