using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visulaGameObjectArray;
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEvnetArgs e) {
        if (e.selectedCounter == baseCounter)
            Show();
        else
            Hide();
    }

    private void Show() {
        foreach (var visulaGameObject in visulaGameObjectArray)
            visulaGameObject.SetActive(true);
    }
    private void Hide() {
        foreach (var visulaGameObject in visulaGameObjectArray)
            visulaGameObject.SetActive(false);
    }
}
