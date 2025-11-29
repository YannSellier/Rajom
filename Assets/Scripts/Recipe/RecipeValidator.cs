using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class RecipeValidator
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES
    #endregion
    
    //=============================================================================
    // VALIDATOR
    //=============================================================================
    
    #region VALIDATOR
   
        
    public static bool IsPartOfRecipes(Part part,  Recipe recipe )
    {   
        if (part == null || recipe == null)
            return false;
        
        if (!recipe.IsPartTypeInRecipe(part.GetPartType()))
            return false;
        
        var recipeModifications = recipe.GetPartModificationsByTypeAndIndex(part.GetPartType(), 0);
        
        if (part.GetModifications().Count != recipeModifications.Count)
            return false;

        for (var i = 0; i < part.GetModifications().Count; i++)
        {
            if (part.GetModifications()[i].GetHeadType() != recipeModifications[i].GetHeadType())
                return false;
        }

        return true; 
        
    }

    #endregion
    
}

