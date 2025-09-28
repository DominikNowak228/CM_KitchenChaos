using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveProgressBarWarningUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
     
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.isFrided() && e.ProgressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }


}
