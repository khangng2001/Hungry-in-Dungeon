using System.Collections.Generic;
using UnityEngine;

public class TestGiveRecipe : MonoBehaviour
{
    [SerializeField] private List<RecipeSO> recipePapers;

    public void GiveRecipe()
    {
        AddRecipe();
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
