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
   
        
    public bool IsPartPartiallyValidForRecipe(Part part,  Recipe recipe )
    {   
        if (part == null || recipe == null)
            return false;
        
        if (!recipe.IsPartTypeInRecipe(part.GetPartType()))
            return false;
        
        var recipeModifications = recipe.GetPartModificationsByTypeAndIndex(part.GetPartType());
        if (recipeModifications == null)
            return false;
        
        if (part.GetModifications().Count > recipeModifications.Count)
            return false;
        
        for (var i = 0; i < part.GetModifications().Count; i++)
        {
            PartModification pm = part.GetModifications()[i];
            PartModification recipeModification = recipeModifications[i];
            if (pm.GetHeadType() != recipeModification.GetHeadType())
                return false;
        }

        return true; 
        
    }
    public bool IsPartFullyValidForRecipe(Part part, Recipe recipe, EPartType desiredPartType)
    {
        if (part == null || recipe == null)
            return false;
        
        if (part.GetPartType() != desiredPartType)
            return false;
        
        if (!recipe.IsPartTypeInRecipe(part.GetPartType()))
            return false;
        
        var recipeModifications = recipe.GetPartModificationsByTypeAndIndex(part.GetPartType());
        if (recipeModifications == null)
            return false;
        
        if (part.GetModifications().Count != recipeModifications.Count)
            return false;
        
        for (var i = 0; i < part.GetModifications().Count; i++)
        {
            PartModification pm = part.GetModifications()[i];
            PartModification recipeModification = recipeModifications[i];
            if (pm.GetHeadType() != recipeModification.GetHeadType())
                return false;
        }

        return true;
    }

    #endregion
    
}

