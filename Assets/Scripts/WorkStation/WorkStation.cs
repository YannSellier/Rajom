using UnityEngine;

public class WorkStation : MonoBehaviour
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

    #endregion

    #region GETTERS / SETTERS

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
        OnHoverExit();
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
        Part part = other.GetComponent<Part>();
        if (part != null)
            SetCurrentPartInTrigger(part);
    }
    private void OnPartTriggerExit(Collider other)
    {
        Part part = other.GetComponent<Part>();
        if (part != null && part == _currentPartInTrigger)
            SetCurrentPartInTrigger(null);
    }
    
    public void OnHoverEnter()
    {
        _hoverVisualObject.SetActive(true);
    }
    public void OnHoverExit()
    {
        _hoverVisualObject.SetActive(false);
    }
    
    #endregion



}
