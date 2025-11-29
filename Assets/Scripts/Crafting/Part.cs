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
    [SerializeField] private List<PartModification> _modifications = new List<PartModification>();

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
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshFilter = gameObject.GetComponent<MeshFilter>();
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
        
        Debug.Log("Part modification added: " + modification.GetHeadType().ToString());
    }

    #endregion

    #region HOVER

    private Color baseColor;
    public void OnHoverEnter()
    {
        baseColor = _meshRenderer.material.color;
        if (_meshRenderer != null)
            _meshRenderer.material.color = Color.yellow;
    }
    public void OnHoverExit()
    {
        if (_meshRenderer != null)
            _meshRenderer.material.color = baseColor;
    }

    #endregion


        
}