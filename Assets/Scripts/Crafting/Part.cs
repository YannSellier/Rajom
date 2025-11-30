using System;
using System.Collections.Generic;
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
    
    #endregion

    #region GETTERS / SETTERS

    public void SetIsGrabbed(bool newStateIsGrabbed)
    {
        _isGrabbed = newStateIsGrabbed;
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
    public float GetAssemblingLength() => _assemblingLength;

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
        int targetModificationCount = GetTargetPartData().GetModifications().Count;
        return stateModificationsVisualObjects.Count - targetModificationCount - 1;
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
        
        if (indexStateVisible < stateModificationsVisualObjects.Count)
            indexStateVisible++;
        
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