using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Transform pressToRebindKeyTransform;

    private void Awake() {
        Instance = this;

        // Obs³uga przycisków
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => Hide());

        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact_Alternate));
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pasue));

    }
    private void Start() {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    public void UpdateVisual() {
        soundEffectsText.text = $"Sound Effect: {Mathf.Round(SoundManager.Instance.GetVolume() * 10)}";
        musicText.text = $"Music: {Mathf.Round(MusicManager.Instance.GetVolume() * 10)}";

        moveUpText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Interact_Alternate);
        pauseText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Pasue);
    }

    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => { 
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) => Hide();
    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);

    private void ShowPressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(true);
    private void HidePressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(false);

}
