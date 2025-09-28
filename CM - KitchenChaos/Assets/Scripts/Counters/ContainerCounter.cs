using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectOS;

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //Player is not carrying anyhitng
            KitchenObject.SpawnKitchenObject(kitchenObjectOS, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
