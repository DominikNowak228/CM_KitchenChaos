using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlterateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    //[SerializeField] private TextMeshProUGUI keyGamepadMoveText;
    //[SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    //[SerializeField] private TextMeshProUGUI keyGamepadInteractAlterateText;
    //[SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownToStart()) {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Interact);
        keyInteractAlterateText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Interact_Alternate);
        keyPauseText.text = GameInput.Instance.GetBingingText(GameInput.Binding.Pasue);
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
