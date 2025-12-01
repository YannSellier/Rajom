using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Part : MonoBehaviour, IGrabbable
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

    [SerializeField] private Transform _visualEvolutionsParent;
    private List<Transform> stateModificationsVisualObjects;

    [SerializeField] private Vector3 _assemblingRotation;
    [SerializeField] private float _assemblingLength = 1f;

    private int indexStateVisible = 0;
    private bool _isGrabbed = false;
    
    private bool _isPlayer1Hovering = false;
    private bool _isPlayer2Hovering = false;
    
    private PartSlot _selectedPartSlot;
    
    #endregion

    #region GETTERS / SETTERS

    public string GetName()
    {
        return "Part_" + GetPartType();
    }

    public void SetIsGrabbed(bool newStateIsGrabbed)
    {
        _isGrabbed = newStateIsGrabbed;
        
        // Disable all colliders
        //foreach (var collider in GetComponentsInChildren<Collider>())
        //    collider.enabled = !_isGrabbed;
    }
    public bool IsGrabbable()
    {
        return !_isGrabbed;
    }

    public Vector3 GetPosition() => transform.position;

    // components
    public Rigidbody GetRigidbody() => _rigidbody;
    
    // part type
    public EPartType GetPartType() => _partData.GetPartType();
    
    // modifications
    public List<PartModification> GetModifications() => _partData.GetModifications();
    public PartData GetTargetPartData()
    {
        RecipesManager recipesManager = RecipesCreator.GetRef().GetRecipesesManager();
        Recipe currentRecipe = recipesManager.GetCurrentRecipe();
        
        PartData targetPartData = currentRecipe.GetPartDataFromPartType(GetPartType());
        return targetPartData;
    }
    
    // assembling
    public Vector3 GetAssemblingRotation() => _assemblingRotation;

    public float GetAssemblingLength() //=> _assemblingLength;
    {
        if (GetPartType() == EPartType.HANDLE || GetPartType() == EPartType.HEAD)
            return 0;
        
        Bounds bounds = PartGrabController.GetObjectMeshBounds(gameObject);
        return bounds.size.z;
    }

    public void SetPartLength(float length)
    {
        _assemblingLength = length;
    }

    #endregion

    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Start()
    {
        CreateComponents();
        initiateStateModifications();
        RefreshHoverStateVisual();
        
        // Find the part slot
        foreach(var slot in RecipesCreator.GetRef().GetRecipesesManager().GetPartsSlots())
            if (slot.GetDesignatedPartType() == GetPartType())
            {
                slot.SetCurrentPart(this);
                _selectedPartSlot = slot;
            }
    }

    private void OnDestroy()
    {
        foreach(HoverController hoverController in FindObjectsOfType<HoverController>())
            hoverController.RemoveGrabbableUnderGrabbable(this);
        
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
        foreach (Transform child in _visualEvolutionsParent)
        {
            stateModificationsVisualObjects.Add(child);
        }
        indexStateVisible = GetDefaultStateIndex();
        refreshState();
    }
    private int GetDefaultStateIndex()
    {
        return 0;
    }
    public void Delete()
    {
        Debug.LogWarning("Part deleted: " + gameObject.name);
        
        _selectedPartSlot?.SetCurrentPart(null);
        onPartDeleted?.Invoke(this);
        
        Destroy(gameObject);
    }

    #endregion

    #region PART MODIFICATIONS

    public void AddModification(PartModification modification)
    {
        _partData.AddModification(modification);
        
        onPartCrafted?.Invoke();

        
        if (indexStateVisible < stateModificationsVisualObjects.Count-1)
            indexStateVisible++;
        
        Recipe recipe = RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe();
        PartData partData = recipe.GetPartDataFromPartType(GetPartType());
        if(indexStateVisible >= partData.GetModifications().Count - 1)
            indexStateVisible = stateModificationsVisualObjects.Count - 1;
        
        refreshState();
        Debug.Log("Part modification added: " + modification.GetHeadType().ToString());
    }
    public void refreshState()
    {
        foreach (Transform stateObject in stateModificationsVisualObjects)
        {
            Boolean verif = stateModificationsVisualObjects[indexStateVisible] == stateObject;
            stateObject.gameObject.SetActive(verif);
        }
    }

    #endregion

    #region HOVER
    
    public void OnHoverEnter(bool isPlayer1)
    {
        if (isPlayer1)
            _isPlayer1Hovering = true;
        else
            _isPlayer2Hovering = true;
        
        RefreshHoverStateVisual();
    }

    public void OnHoverExit(bool isPlayer1)
    {
        if (isPlayer1)
            _isPlayer1Hovering = false;
        else
            _isPlayer2Hovering = false;
        
        RefreshHoverStateVisual();
    }

    private void RefreshHoverStateVisual()
    {
        _selectionVisualObject.SetActive(_isPlayer1Hovering || _isPlayer2Hovering);
    }
    
    #endregion


        
}