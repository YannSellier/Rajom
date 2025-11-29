using System;
using System.Collections.Generic;
using NUnit.Framework;

public class RecipesManager
{
    //=============================================================================
    // VARIABLES
    //=============================================================================
    
    #region VARIABLES

    private List<Recipe> _recipes;
    private RecipeValidator _recipeValidator = new RecipeValidator();
    private int _currentRecipeIndex = -1;
    
    #endregion

    #region GETTERS / SETTERS


    public RecipeValidator GetRecipeValidator() => _recipeValidator;
    public Recipe GetCurrentRecipe()
    {
        if (_recipes.Count == 0)
            return null;

        if (_currentRecipeIndex == -1)
            _currentRecipeIndex = 0;

        return _recipes[_currentRecipeIndex];
    }
    public void PickNextRecipe()
    {
        _currentRecipeIndex = (_currentRecipeIndex + 1) % _recipes.Count;
    }
    
    #endregion
    


    //=============================================================================
    // CONSTRUCTOR
    //=============================================================================

    public RecipesManager(List<Recipe> recipes)
    { 
        _recipes = recipes ?? new List<Recipe>();
    }
    
    //=============================================================================
    // GETTERS / SETTERS
    //=============================================================================
    
    #region GETTERS / SETTERS

    public List<Recipe> GetRecipes() => _recipes; 
    
    public void PushRecipe(Recipe recipe)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));

        _recipes.Add(recipe);
    }
    
    #endregion
}
