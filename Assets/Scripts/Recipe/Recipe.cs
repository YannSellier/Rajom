
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class PartPrefab
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int numberSteps;
    [SerializeField] public EPartType partType;
}
[Serializable]
public class Recipe
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] 
    private string _name;

    [SerializeField] 
    private string _description;

    [SerializeField] 
    private List<PartData> _parts;

    [SerializeField] public GameObject[] partPrefabs = new  GameObject[4];

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

    public string GetName() => _name; 
    public string GetDescription() => _description; 
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