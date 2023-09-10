using System.Collections.Generic;
using UnityEngine;

public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] private RecipePaperUI recipePaperPrefab;
    [SerializeField] private RectTransform contentPanel;

    List<RecipePaperUI> listOfPaperUI = new List<RecipePaperUI>();

    [SerializeField] private RecipeDescriptionUI recipeDescription;

    [SerializeField] private TestGiveRecipe testGiveRecipe;

    private void Awake()
    {
        recipeDescription.ResetDescription();
    }

    //RecipeSO
    public void AddRecipe()
    {
        RecipePaperUI paperUI = Instantiate(recipePaperPrefab, Vector3.zero, Quaternion.identity);
        paperUI.transform.SetParent(contentPanel);
        paperUI.transform.localScale = Vector3.one;
        paperUI.recipeSO = testGiveRecipe.GiveRecipeSO();   //gan recipeSO vao paperUI
        paperUI.SetData(paperUI.recipeSO.RecipeImage);
        listOfPaperUI.Add(paperUI);

        paperUI.OnPaperClicked += PaperUI_OnPaperClicked;
    }

    private void PaperUI_OnPaperClicked(RecipePaperUI obj)
    {
        ResetSelection();
        Debug.Log("ItemClicked!!");
        Debug.Log(obj.transform.position);
        recipeDescription.SetDescription(obj.recipeSO.RecipeImage, obj.recipeSO.Title, obj.recipeSO.Benefit, obj.recipeSO.Ingredient);
        obj.Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    private void ResetSelection()
    {
        recipeDescription.ResetDescription();
        DeSelectionAllItems();
    }

    private void DeSelectionAllItems()
    {
        foreach (RecipePaperUI item in listOfPaperUI)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
