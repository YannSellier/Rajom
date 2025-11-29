
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class Recipe
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private string _name;
    private string _description;
    private List<TempPart> _parts;

    #endregion

    //=============================================================================
    // CONSTRUCTOR
    //=============================================================================

    public Recipe(string name, string description, List<TempPart> parts)
    {
        _name = name;
        _description = description;
        _parts = parts ?? new List<TempPart>();
    }
    
    //=============================================================================
    // GETTERS / SETTERS
    //=============================================================================
    
    #region GETTERS / SETTERS

    public List<TempPart> GetParts() => _parts; 
    
    #endregion

    
    public bool IsPartTypeInRecipe(EPartType partType)
    {
        return _parts.Any(p => p.Type == partType);
    }
    
    
    public List<string> GetPartModificationsByTypeAndIndex(EPartType partType, int index)
    {
        if (index < 0 || index >= _parts.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index hors limites de la liste des parts.");

        return _parts[index].Modifications;
    }

    
}