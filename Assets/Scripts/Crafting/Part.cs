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
    private Rigidbody _rigidbody;

    #endregion

    #region VARIABLES

    [SerializeField] private PartData _partData = new PartData();
    
    public Action<Part> onPartDeleted;
    public Action onPartCrafted;

    [SerializeField] private List<Transform> stateModifications;

    private int indexStateVisible = 0;
    #endregion

    #region GETTERS / SETTERS

    
    // components
    public Rigidbody GetRigidbody() => _rigidbody;
    
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
        initiateStateModifications();
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
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void initiateStateModifications()
    {
        stateModifications = new List<Transform>();
        foreach (Transform child in transform)
        {
            stateModifications.Add(child);
        }
        indexStateVisible = 0;
        refreshState();
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
        
        indexStateVisible++;
        refreshState();
        Debug.Log("Part modification added: " + modification.GetHeadType().ToString());
    }

    public void refreshState()
    {
        {
            foreach (Transform stateObject in stateModifications)
            {
                Boolean verif = stateModifications[indexStateVisible] == stateObject;
                stateObject.gameObject.SetActive(verif);
            }
        }
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