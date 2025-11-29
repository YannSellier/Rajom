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
   
        
    public static bool IsPartOfRecipes(TempPart part,  Recipe recipe )
    {   
        if (part == null || recipe == null)
            return false;
        
        if (!recipe.IsPartTypeInRecipe(part.Type))
            return false;
        
        var recipeModifications = recipe.GetPartModificationsByTypeAndIndex(part.Type, 0);
        
        if (part.Modifications.Count != recipeModifications.Count)
            return false;

        for (var i = 0; i < part.Modifications.Count; i++)
        {
            if (part.Modifications[i] != recipeModifications[i])
                return false;
        }

        return true; 
        
    }

    #endregion
    
}

