using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartData 
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region COMPONENTS

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;

    #endregion

    #region VARIABLES

    [SerializeField] private EPartType _partType;
    [SerializeField] private List<PartModification> _modifications;
    
    #endregion

    #region GETTERS / SETTERS

    // part type
    public EPartType GetPartType() => _partType;
    
    // modifications
    public List<PartModification> GetModifications() => _modifications;

    #endregion

    #region CONSTRUCTOR
    
    public PartData(EPartType partType, List<PartModification> partModifications)
    { 
        _partType = partType;
        _modifications = partModifications;
    }
    public PartData()
    {
        _modifications = new List<PartModification>();
    }
    
    #endregion
    

    #region PART MODIFICATIONS

    public void AddModification(PartModification modification)
    {
        _modifications.Add(modification);
    }

    #endregion
}