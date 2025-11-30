using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartGrabController : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private Transform _grabPosition;
    [SerializeField] private EInput _grabInput = EInput.Grab1;

    [SerializeField] private TriggerCollider _detectorTriggerCollider;
    
    private PlayerInput _playerInput;
    private Part _currentGrabbedPart;
    private Part _currentPartUnderHead;
    
    #endregion

    #region GETTERS / SETTERS

    public Part GetCurrentGrabbedPart()
    {
        return _currentGrabbedPart;
    }

    public Part GetCurrentPartUnderHead()
    {
        return _currentPartUnderHead;
    }

    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    /* Normalement plus nécessaire
     
     protected void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.actions[_grabInput.ToString()].performed += ctx => OnGrabInput();
        
        _detectorTriggerCollider.onTriggerEnter += OnDetectorTriggerEnter;
        _detectorTriggerCollider.onTriggerExit += OnDetectorTriggerExit;
    }

    #endregion



    //=============================================================================
    // GRAB CONTROLLER
    //=============================================================================

    #region GRAB CONTROLLER

    public void callOnGrabInput()
    {
        OnGrabInput();
    }
    
    private void OnGrabInput()
    {
        if (_currentGrabbedPart != null)
        {
            ReleasePart(_currentGrabbedPart);
            _currentGrabbedPart = null;
        }
        else
        {
            TryGrabbingPartUnderHead();
        }
    }
    private void TryGrabbingPartUnderHead()
    {
        if (_currentPartUnderHead != null)
        {
            GrabPart(_currentPartUnderHead);
            _currentGrabbedPart = _currentPartUnderHead;
            _currentPartUnderHead = null;
        }
    }
    private void GrabPart(Part part)
    {
        if (part == null || _grabPosition == null)
            return;
        
        part.transform.SetParent(_grabPosition);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = Quaternion.identity;

        part.GetRigidbody().useGravity = false;
        part.GetRigidbody().isKinematic = true;
    }
    private void ReleasePart(Part part)
    {
        if (part == null)
            return;

        part.transform.SetParent(null);
        part.GetComponent<Rigidbody>().useGravity = true;
        part.GetComponent<Rigidbody>().isKinematic = false;
    }

    #endregion

    #region PART UNDER HEAD

    private void SetCurrentPartUnderHead(Part part)
    {
        if (_currentPartUnderHead != null)
            _currentPartUnderHead.OnHoverExit();
        
        _currentPartUnderHead = part;
        
        if (_currentPartUnderHead != null)
            _currentPartUnderHead.OnHoverEnter();
    }

    #endregion

    #region DETECTOR

    private void OnDetectorTriggerEnter(Collider other)
    {
        if (other == null)
            return;
        
        Part part = other.GetComponent<Part>();
        if (part == null)
            return;
        
        SetCurrentPartUnderHead(part);
    }
    private void OnDetectorTriggerExit(Collider other)
    {
        if (other == null)
            return;
        
        Part part = other.GetComponent<Part>();
        if (part == null)
            return;
        
        if (part == _currentPartUnderHead)
            SetCurrentPartUnderHead(null);
    }


    #endregion


        


}