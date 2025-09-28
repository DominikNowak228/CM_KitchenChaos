using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawnd;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;


    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulrecipesAmount = 0;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Start() {
        spawnRecipeTimer = 0;
    }
    private void Update() {
        spawnRecipeTimer += Time.deltaTime;

        if (GameManager.Instance.IsGamePlaying() && spawnRecipeTimer > spawnRecipeTimerMax) {
            spawnRecipeTimer = 0;

            if (waitingRecipeSOList.Count < waitingRecipeMax) {
                RecipeSO recipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(recipeSO);

                OnRecipeSpawnd?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectList().Count) {
                // Has the same number of ingredients

                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenSO in waitingRecipeSO.KitchenObjectSOList) {
                    // Cuclingthrough all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateRecipeKitchenSO in plateKitchenObject.GetKitchenObjectList()) {
                        // Cuclingthrough all ingredients in the plate
                        if (plateRecipeKitchenSO == recipeKitchenSO) {
                            ingredientFound = true; break;
                        }
                    }
                    if (!ingredientFound) {
                        //This recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe) {
                    // Player deliverd the correct recipe
                    successfulrecipesAmount -= -1;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;

                }
            }
        }
        //No matches found!
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }
    public int GetSuccessfulrecipesAmount() {
        return successfulrecipesAmount;
    }
}
