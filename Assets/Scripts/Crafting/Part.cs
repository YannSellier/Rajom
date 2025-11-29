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

    [SerializeField] private PartData _partData = new PartData();
    
    public Action<Part> onPartDeleted;
    public Action onPartCrafted;

    #endregion

    #region GETTERS / SETTERS

    // part type
    public EPartType GetPartType() => _partData.GetPartType();
    
    // modifications
    public List<PartModification> GetModifications() => _partData.GetModifications();

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
        Debug.LogWarning("Part deleted: " + gameObject.name);
        
        onPartDeleted?.Invoke(this);
        
        Destroy(gameObject);
    }

    #endregion

    #region PART MODIFICATIONS

    public void AddModification(PartModification modification)
    {
        _partData.AddModification(modification);
        
        onPartCrafted?.Invoke();
        
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