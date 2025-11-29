using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region COMPONENTS

    private Rigidbody _rigidbody;

    #endregion

    #region VARIABLES

    [SerializeField] private PartData _partData = new PartData();

    [SerializeField] private GameObject _selectionVisualObject;

    public Action<Part> onPartDeleted;
    public Action onPartCrafted;

    private List<Transform> stateModificationsVisualObjects;

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
        OnHoverExit();
    }

    #endregion
    
    //=============================================================================
    // PART
    //=============================================================================

    #region PART

    private void CreateComponents()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void initiateStateModifications()
    {
        stateModificationsVisualObjects = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child == _selectionVisualObject.transform)
                continue;
            
            stateModificationsVisualObjects.Add(child);
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
            foreach (Transform stateObject in stateModificationsVisualObjects)
            {
                Boolean verif = stateModificationsVisualObjects[indexStateVisible] == stateObject;
                stateObject.gameObject.SetActive(verif);
            }
        }
    }

    #endregion

    #region HOVER

    private Color baseColor;
    public void OnHoverEnter()
    {
        _selectionVisualObject.SetActive(true);
    }
    public void OnHoverExit()
    {
        _selectionVisualObject.SetActive(false);
    }

    #endregion


        
}