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
