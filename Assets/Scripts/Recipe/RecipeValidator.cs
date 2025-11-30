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
        
        if (!CompareModificationsLists(part.GetModifications(), recipeModifications, false))
            return false;

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
        
        if (!CompareModificationsLists(part.GetModifications(), recipeModifications, true))
            return false;

        return true;
    }
    private bool CompareModificationsLists(List<PartModification> partModifications, List<PartModification> recipeModifications, bool shouldCompareCount)
    {
        if (partModifications.Count != recipeModifications.Count && shouldCompareCount)
            return false;

        for (int i = 0; i < partModifications.Count; i++)
        {
            if (!CompareModifications(partModifications[i], recipeModifications[i]))
                return false;
        }

        return true;
    }
    private bool CompareModifications(PartModification pm1, PartModification pm2)
    {
        if (pm1.GetHeadType() != pm2.GetHeadType())
            return false;
        
        if (pm1.GetWorkStationType() != pm2.GetWorkStationType())
            return false;

        return true;
    }

    #endregion
    
}

