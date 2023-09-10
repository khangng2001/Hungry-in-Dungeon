using System.Collections.Generic;
using UnityEngine;

public class TestGiveRecipe : MonoBehaviour
{
    [SerializeField] private List<RecipeSO> recipePapers;

    public RecipeSO RandomRecipeSO()
    {
        RecipeSO temp = new RecipeSO();
        temp = recipePapers[0];
        recipePapers.Remove(recipePapers[0]);
        return temp;
    }

    public RecipeSO GiveRecipeSO()
    {
        return RandomRecipeSO();
    }
}
