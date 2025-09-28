using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;

        public OnStateChangedEventArgs(State state) {
            this.state = state;
        }
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
    }

    private void Update() {

        switch (state) {
            case State.Idle:

                break;
            case State.Frying:

                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(fryingTimer, fryingRecipeSO.fryingTimerMax));
                if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                    fryingTimer = 0f;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                    state = State.Fried;
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().getKitchenObjectSO());

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));

                }
                break;
            case State.Fried:

                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(burningTimer, burningRecipeSO.burningTimerMax));
                if (burningTimer > burningRecipeSO.burningTimerMax) {
                    burningTimer = 0f;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    state = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));

                }
                break;
            case State.Burned:

                break;
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is on KitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().getKitchenObjectSO())) {

                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().getKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(fryingTimer, fryingRecipeSO.fryingTimerMax));


                }
            } else {
                //Player is not carrying anything
            }
        } else {
            // There is a KtichenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().getKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0, 1));

                    }
                }
            } else {
                //Player isn't carrying anyhing
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0, 1));

            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectsSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectsSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutpubForInput(KitchenObjectSO inputKitchenObjectSO) {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO == null ? null : fryingRecipeSO.output;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var fryingRecipeSO in fryingRecipeSOArray)
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var burningRecpieSO in burningRecipeSOArray)
            if (burningRecpieSO.input == inputKitchenObjectSO)
                return burningRecpieSO;
        return null;
    }

    public bool isFrided() => state == State.Fried;
}
