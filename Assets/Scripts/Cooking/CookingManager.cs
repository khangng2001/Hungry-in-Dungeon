using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager instance;

    public event EventHandler<OnProgressBarChangedEventArgs> OnProgressBarChanged;
    public class OnProgressBarChangedEventArgs : EventArgs
    {
        public float progressBarNormalized;
    }

    [SerializeField] private float cookingProgressMax;
    [SerializeField] private float cookingProgressTimer;

    [SerializeField] private GameObject dropItemZone;
    [SerializeField] private CookingUI cookingUI;
    [SerializeField] private GameObject interactUI;

    public CookingSlot[] cookingSlots;
    [SerializeField] private GameObject resultSlot;
    [SerializeField] private GameObject cookBtn;
    [SerializeField] private GameObject cookBar;
    [SerializeField] private bool open = false;   //check resultSlot open or not

    public List<ItemSO> itemList;
    [SerializeField] private string[] recipes;
    [SerializeField] private ItemSO[] recipeResults;
    [SerializeField] private GameObject inventoryItemPrefab;

    private string tempRecipeString;
    private string tempRecipeStringS;

    private enum State {
        Idle,       //when nothing cooking
        Cooking,    //when the player click CookBtn
        Cooked      //when the CookBar is full
    }
    private State state;

    private void Awake()
    {
        //MakeSingleton();
        cookingUI.Hide();
        interactUI.SetActive(false);
        cookBtn.SetActive(false);
    }

    void MakeSingleton()
    {
        /*if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }*/
    }

    [SerializeField] private float distanceToPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject campfire;
    private bool CheckDistance()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, campfire.transform.position);
        if (distanceToPlayer <= 2f)
        {
            interactUI.SetActive(true);
            return true;
        } else
        {
            interactUI.SetActive(false);
            return false;
        }
    }

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (CheckDistance())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (cookingUI.isActiveAndEnabled == false)
                {
                    cookingUI.Show();
                    open = true;
                    dropItemZone.SetActive(false);
                }
                else
                {
                    cookingUI.Hide();
                    open = false;
                    dropItemZone.SetActive(true);
                }
            }
        } else
        {
            cookingUI.Hide();
        }

        //run CookBar
        if (open)
        {
            dropItemZone.SetActive(false);
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Cooking:
                    cookBtn.SetActive(false);

                    //update CookBarUI
                    cookingProgressTimer += Time.deltaTime;
                    OnProgressBarChanged?.Invoke(this, new OnProgressBarChangedEventArgs
                    {
                        progressBarNormalized = cookingProgressTimer / cookingProgressMax
                    });

                    if (cookingProgressTimer > cookingProgressMax)
                    {
                        //spawn food
                        InventoryItem itemInSlot = resultSlot.GetComponentInChildren<InventoryItem>();
                        for (int i = 0; i < recipes.Length; i++)
                        {
                            if (recipes[i] == tempRecipeStringS)
                            {
                                if (itemInSlot == null)
                                    SpawnFood(recipeResults[i]);
                            }
                        }

                        //switch State
                        state = State.Cooked;
                    }
                    break;
                case State.Cooked:
                    //update UI
                    cookingProgressTimer = 0f;
                    OnProgressBarChanged?.Invoke(this, new OnProgressBarChangedEventArgs
                    {
                        progressBarNormalized = cookingProgressTimer
                    });

                    //switch State
                    state = State.Idle;
                    break;
            }
        }

        CheckForCreatedRecipe();
        CheckItemEqualZero();
    }

    private void CheckForCreatedRecipe()
    {
        if (open)
        {
            for (int i = 0; i < cookingSlots.Length; i++)
            {
                InventoryItem itemInSlot = cookingSlots[i].GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    itemList[i] = itemInSlot.item;
                }
            }

            string currentRecipeString = "";
            foreach (ItemSO item in itemList)
            {
                if (item != null)
                {
                    currentRecipeString += item.id;
                } else {
                    currentRecipeString += "null";
                }
            }

            for (int i = 0; i < recipes.Length; i++)
            {
                InventoryItem itemInSlot = resultSlot.GetComponentInChildren<InventoryItem>();
                if (recipes[i] == currentRecipeString)
                {
                    if (itemInSlot == null)
                    {
                        cookBtn.SetActive(true);
                    }
                }
                if (itemInSlot != null || tempRecipeString != currentRecipeString || cookBar.activeInHierarchy == true)
                {
                    cookBtn.SetActive(false);
                }
            }
            tempRecipeString = currentRecipeString;
        }
    }

    public void SpawnFood(ItemSO food)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, resultSlot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(food);
    }

    public void Cooking()
    {
        cookBtn.SetActive(false);   //hide cookButton
        tempRecipeStringS = tempRecipeString;
        state = State.Cooking;      //switch state
        cookingProgressTimer = 0f;
        OnClickResultSlot();        //sub ingredients
    }

    public void OnClickResultSlot() //ingrediens--
    {
        for(int i = 0; i < cookingSlots.Length; i++)
        {
            InventoryItem itemInSlot = cookingSlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
        }
        CheckForCreatedRecipe();
    }

    public void CheckItemEqualZero()    //check if itemInSlot is null -> remove ItemSO in itemList
    {
        for (int i = 0; i < cookingSlots.Length; i++)
        {
            InventoryItem itemInSlot = cookingSlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                itemList[i] = null;
            }
        }
    }
}
