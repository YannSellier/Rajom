using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private EPartType _headType;
    [SerializeField] private List<PartModification> _modifications;

    public Action<EPartType> onPartDeleted;

    #endregion

    #region GETTERS / SETTERS

    // part type
    public EPartType GetPartType() => _headType;
    
    // modifications
    public List<PartModification> GetModifications() => _modifications;

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // PART
    //=============================================================================

    #region PART

    public void Delete()
    {
        onPartDeleted?.Invoke(_headType);
        
        Destroy(gameObject);
    }

    #endregion

    #region PART MODIFICATIONS

    public void AddModification(PartModification modification)
    {
        _modifications.Add(modification);
    }

    #endregion

        
}