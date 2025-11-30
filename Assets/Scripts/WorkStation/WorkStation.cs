using UnityEngine;

public class WorkStation : MonoBehaviour, IGrabbable
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    // components
    [SerializeField] private TriggerCollider _partTriggerCollider;
    [SerializeField] private GameObject _hoverVisualObject;
    
    // properties
    private Part _currentPartInTrigger;
    [SerializeField] private EWorkStationType _workStationType;
    
    private bool _isPlayer1Hovering = false;
    private bool _isPlayer2Hovering = false;

    #endregion

    #region GETTERS / SETTERS

    public string GetName()
    {
        return "WorkStation" + _workStationType;
    }
    public bool IsGrabbable()
    {
        return true;
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public EWorkStationType GetWorkStationType() => _workStationType;
    public Part GetCurrentPartInTrigger() => _currentPartInTrigger;

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        // subscribe to trigger collider events
        _partTriggerCollider.onTriggerEnter += OnPartTriggerEnter;
        _partTriggerCollider.onTriggerExit += OnPartTriggerExit;
        
        SetCurrentPartInTrigger(null);
        RefreshHoverStateVisual();
        
    }

    #endregion



    //=============================================================================
    // WORK STATION
    //=============================================================================

    #region WORK STATION

    public bool IsInteractable()
    {
        return _currentPartInTrigger != null;
    }
    private void SetCurrentPartInTrigger(Part part)
    {
        if (part == _currentPartInTrigger)
            return;
        
        _currentPartInTrigger = part;
    }

    #endregion

    #region CALLBACKS

    private void OnPartTriggerEnter(Collider other)
    {
        Part part = other.GetComponentInParent<Part>();
        if (part != null)
            SetCurrentPartInTrigger(part);
    }
    private void OnPartTriggerExit(Collider other)
    {
        Part part = other.GetComponentInParent<Part>();
        if (part != null && part == _currentPartInTrigger)
            SetCurrentPartInTrigger(null);
    }
    private void RefreshHoverStateVisual()
    {
        _hoverVisualObject.SetActive(_isPlayer1Hovering || _isPlayer2Hovering);
    }
    
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
    
    #endregion



}
