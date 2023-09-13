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
    
    private static int sceneIndex = 2;
    [SerializeField] private Vector3[] entrances;
    [SerializeField] private int currentIndexEntrance = 0;

    [SerializeField] private GameObject blackCurtain;
    
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

    private void Update()
    {
        Debug.Log("currentIndex: " + currentIndexEntrance);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
    }
    

    private void PlayBackgroundMusic()
    {
        if (SceneManager.GetActiveScene().name == "Scene_01")
        {
            AudioManager.Instance.PlayBackgroundMusic();
            AudioManager.Instance.StopDungeonMusic();
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlayDungeonMusic();
        }
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
    public void ProceedScene()
    {
        StartCoroutine(ProceedWaitForBlackCurtain());
    }
    
    
    
    public void GoPreviousScene()
    {
        StartCoroutine(PreviousWaitForBlackCurtain());
    }
    
    private IEnumerator ProceedWaitForBlackCurtain()
    {
        sceneIndex++;
        currentIndexEntrance++;
        blackCurtain.SetActive(true);
        PlayerController.instance.transform.position = entrances[currentIndexEntrance];
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
        yield return new WaitForSeconds(1f);
        blackCurtain.SetActive(false);
        
    }

    private IEnumerator PreviousWaitForBlackCurtain()
    {
        currentIndexEntrance--;
        sceneIndex--;
        blackCurtain.SetActive(true);
        PlayerController.instance.transform.position = entrances[currentIndexEntrance];
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
        yield return new WaitForSeconds(1f);
        blackCurtain.SetActive(false);
        
    }
    
    //=================SAVE AND LOAD DATA================
    public void LoadData(GameData data)
    {
        sceneIndex = data.Scene;
    }

    public void SaveData(ref GameData data)
    {
        data.Scene = sceneIndex;
    }

}
