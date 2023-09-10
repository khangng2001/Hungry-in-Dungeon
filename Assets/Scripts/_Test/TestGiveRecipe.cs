using System.Collections.Generic;
using UnityEngine;

public class TestGiveRecipe : MonoBehaviour
{
    [SerializeField] private List<RecipeSO> recipePapers;

    public void AddRecipe()
    {
        RecipeManager.instance.AddRecipe(recipePapers[0]);
        recipePapers.Remove(recipePapers[0]);
        GameManager.instance.SaveDataRecipe();

        if (recipePapers.Count == 0)
        {
            //unactive this script
        }
    }
}
