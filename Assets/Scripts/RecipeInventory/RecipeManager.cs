using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private RecipeBookUI recipeBookUI;

    private void Awake()
    {
        recipeBookUI.Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (recipeBookUI.isActiveAndEnabled == false)
            {
                recipeBookUI.Show();
            }
            else
            {
                recipeBookUI.Hide();
            }
        }
    }
}
