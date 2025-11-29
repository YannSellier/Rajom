using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region COMPONENTS

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;

    #endregion

    #region VARIABLES

    [SerializeField] private EPartType _headType;
    [SerializeField] private List<PartModification> _modifications;

    public Action<Part> onPartDeleted;

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
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Start()
    {
        CreateComponents();
    }

    #endregion



    //=============================================================================
    // PART
    //=============================================================================

    #region PART

    private void CreateComponents()
    {
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshFilter = gameObject.AddComponent<MeshFilter>();
    }
    public void Delete()
    {
        onPartDeleted?.Invoke(this);
        
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