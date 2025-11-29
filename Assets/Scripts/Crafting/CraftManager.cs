using UnityEngine;

public class CraftManager
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES


    #endregion

    #region STATIC

    private static CraftManager _instance;
    public static CraftManager GetRef()
    {
        if (_instance == null)
            _instance = new CraftManager();
        return _instance;
    }

    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // CRAFTING
    //=============================================================================

    #region CRAFTING

    public bool CraftPart(Part part, Head head)
    {
        if (part == null || head == null)
            return false;

        PartModification modification = CreatePartModification(head);
        part.AddModification(modification);

        if (!ValidateCraftingResult(part))
        {
            part.Delete();
            return false;
        }

        return true;
    }
    
    private bool ValidateCraftingResult(Part resultPart)
    {
        RecipesManager recipesManager = RecipesCreator.GetRef().GetRecipesesManager();
        RecipeValidator recipeValidator = recipesManager.GetRecipeValidator();
        
        bool result = recipeValidator.IsPartPartiallyValidForRecipe(resultPart, recipesManager.GetCurrentRecipe());
        
        if (result == false)
            Debug.LogWarning("Crafting failed: Part does not match the recipe.");
        
        return result;
    }
    private PartModification CreatePartModification(Head head)
    {
        return new PartModification(head.GetHeadType());
    }

    #endregion
        
}