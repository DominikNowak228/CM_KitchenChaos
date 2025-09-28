using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
        public OnIngredientAddedEventArgs(KitchenObjectSO kitchenObjectSO) {
            this.kitchenObjectSO = kitchenObjectSO;
        }
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsSoList;
    private List<KitchenObjectSO> KitchenObjectSOList;

    private void Awake() {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {

        if (!validKitchenObjectsSoList.Contains(kitchenObjectSO)) return false;
        if (KitchenObjectSOList.Contains(kitchenObjectSO)) return false;

        KitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(kitchenObjectSO));
        return true;
    }
    public List<KitchenObjectSO> GetKitchenObjectList() {
        return KitchenObjectSOList;
    }
}
