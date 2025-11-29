
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
    private List<PartData> _parts;

    #endregion

    //=============================================================================
    // CONSTRUCTOR
    //=============================================================================

    public Recipe(string name, string description, List<PartData> parts)
    {
        _name = name;
        _description = description;
        _parts = parts ?? new List<PartData>();
    }
    
    //=============================================================================
    // GETTERS / SETTERS
    //=============================================================================
    
    #region GETTERS / SETTERS

    public List<PartData> GetParts() => _parts; 
    
    #endregion

    
    public bool IsPartTypeInRecipe(EPartType partType)
    {
        return _parts.Any(p => p.GetPartType() == partType);
    }


    public PartData GetPartDataFromPartType(EPartType partType)
    {
        foreach (var part in _parts)
            if (part.GetPartType() == partType)
                return part;

        return null;
    }
    public List<PartModification> GetPartModificationsByTypeAndIndex(EPartType partType)
    {
        PartData partData = GetPartDataFromPartType(partType);
        if (partData != null)
            return partData.GetModifications();
        return new List<PartModification>();
    }

    
}