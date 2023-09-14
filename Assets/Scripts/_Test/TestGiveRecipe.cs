using System.Collections.Generic;
using UnityEngine;

public class TestGiveRecipe : MonoBehaviour
{
    [SerializeField] private List<RecipeSO> recipePapers;

    public void GiveRecipe()
    {
        if (RecipeManager.instance.listOfPaperUI.Count == 0)
        {
            AddRecipe();
        } else if (RecipeManager.instance.listOfPaperUI.Count > 0) {
            if (PlayerController.instance.GetCointPaper() > 0)
            {
                AddRecipe();
                PlayerController.instance.DecreaseCoinPaper();
            } else
            {
                Debug.Log("You dont have enough coinPaper");
            }
        }
    }
    private void AddRecipe()
    {
        if (recipePapers.Count > 0)
        {
            RecipeManager.instance.AddRecipe(recipePapers[0]);
            GameManager.instance.SaveDataRecipe(recipePapers[0]);
            recipePapers[0] = null;
        }
    }
}
