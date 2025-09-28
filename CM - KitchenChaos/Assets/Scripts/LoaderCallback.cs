using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFisrstUpdate = true;

    private void Update() {
        if (isFisrstUpdate) {
            isFisrstUpdate=false;

            Loader.LoaderCallback();
        }
    }
}
