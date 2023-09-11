using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Inventory")]
    public List<ItemSO> item;   //item.length = inventorySlot.length = 7
    public List<int> count;
    [Header("Recipe")]
    public List<RecipeSO> recipes;
    [SerializeField] private int sceneIndex = 0;
    [SerializeField] private Vector3[] entrances;
    [SerializeField] private int currentIndexEntrance = 0;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        currentIndexEntrance = 0;
    }

    private void OnDisable()
    {
        currentIndexEntrance = 0;
    }

    public void SaveDataInventory()
    {
        for (int i = 0; i < item.Count; i++)
        {
            item[i] = InventoryManager.instance.SaveDataItem(i);
            count[i] = InventoryManager.instance.SaveDataCount(i);
        }
    }
    public void LoadDataInventory()
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i] != null)
            {
                InventoryManager.instance.LoadData(i, item[i], count[i]);
            }
        }
    }
    public void SaveDataRecipe(RecipeSO recipeSO)
    {
        recipes.Add(recipeSO);
        /*for (int i = 0; i < recipes.Count; i++)
        {
            recipes[i] = RecipeManager.instance.SaveDataRecipe(i);
            recipes.Add(recipes[i]);
        }*/
    }
    public void LoadDataRecipe()
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i] != null)
            {
                RecipeManager.instance.AddRecipe(recipes[i]);
            }
        }
    }

    //CHANGE SCENE
    public void ChangeScene()
    {
        sceneIndex++;
        SceneManager.LoadScene(sceneIndex);
        PlayerController.instance.transform.position = entrances[currentIndexEntrance];
        currentIndexEntrance++;
        if (currentIndexEntrance > entrances.Length)
        {
            currentIndexEntrance = currentIndexEntrance;
        }
    }
    
    
   

   
}
