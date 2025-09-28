using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;

    // Event odpowiedzialny za wyznaczanie obiektu do interackji
    public event EventHandler<OnSelectedCounterChangedEvnetArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEvnetArgs : EventArgs
    {
        public BaseCounter selectedCounter;
        public OnSelectedCounterChangedEvnetArgs(BaseCounter baseCounter) {
            this.selectedCounter = baseCounter;
        }
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;


    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if (Instance != null)
            Debug.LogError("There is more then one Player instance");
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }

    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    // Obs³uga interakcji
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter)) {
                // Has ClearCounter
                if (baseCounter != selectedCounter)
                    SetSelectedCounter(baseCounter);
            } else
                SetSelectedCounter(null);
        } else
            SetSelectedCounter(null);


    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Weryfikacja, czy mo¿e siê ruszyæ

        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // Cannot move towards moveDir
            // Nie mo¿na siê ruszyæ w kierunku moveDir

            // Attempt only X movement
            // Próba ruchu tylko w osi X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                // Can move only on the X
                // Mo¿na poruszaæ siê tylko w osi X
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X
                // Nie mo¿na poruszaæ siê tylko w osi X

                // Attempt only Z movement
                // Próba ruchu tylko w osi Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    // Can move only on the Z
                    // Mo¿na poruszaæ siê tylko w osi Z
                    moveDir = moveDirZ;
                } else {
                    // Cannot move in any direction
                    // Nie mo¿na poruszaæ siê w ¿adnym kierunku
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        // Przypisywanie wartoœci do isWalking potrzebnej do animowania ruchu
        isWalking = moveDir != Vector3.zero;

        // Rotacja gracza
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEvnetArgs(selectedCounter));
    }

    public Transform GetKitchenObjectFollowTransform() {
        return KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
