using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button repeatButton;


    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        menuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainManuScene); });
        repeatButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.GameScenes); });

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();

            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulrecipesAmount().ToString();
        } else {
            Hide();
        }
    }

    private void Hide(int rizzLevel = 0) {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
