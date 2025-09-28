using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
        public OnProgressChangedEventArgs() { }
        public OnProgressChangedEventArgs(float currnetValue, float maxValue) {
            ProgressNormalized = currnetValue / maxValue;
        }
    }
}
