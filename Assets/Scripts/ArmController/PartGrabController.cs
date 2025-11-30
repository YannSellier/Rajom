using DefaultNamespace;
using DefaultNamespace.ArmController;
using DefaultNamespace.UI;
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

    [SerializeField] private HoverController _hoverController;
    [SerializeField] private EArmPosition _armPosition;
    private PlayerInput _playerInput;
    private Part _currentGrabbedPart;
    private HoldingPartUI _holdingPartUI;
    
    #endregion

    #region GETTERS / SETTERS

    public Part GetCurrentGrabbedPart()
    {
        return _currentGrabbedPart;
    }

    public Part GetCurrentPartUnderHead()
    {
        return _hoverController.GetClosestGrabbableOfType<Part>();
    }
    public void SetCurrentGrabbedPart(Part newGrabbedPart)
    {
        if (_currentGrabbedPart != null)
        {
            _hoverController.RemoveGrabbableToIgnore(_currentGrabbedPart);
            _currentGrabbedPart.SetIsGrabbed(false);
        }

        _currentGrabbedPart = newGrabbedPart;

        if (_currentGrabbedPart != null)
        {
            _hoverController.AddGrabbableToIgnore(newGrabbedPart);
            _currentGrabbedPart.SetIsGrabbed(true);
        }
    }

    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

     
     protected void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _holdingPartUI = FindObjectOfType<HoldingPartUI>();
        //_playerInput.actions[_grabInput.ToString()].performed += ctx => OnGrabInput();
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
            _holdingPartUI.VoidHoldingPartUI(_armPosition);
        }
        else
        {
            TryGrabbingPartUnderHead();
        }
    }
    private void TryGrabbingPartUnderHead()
    {
        Part currentPartUnderHead = GetCurrentPartUnderHead();
        if (currentPartUnderHead != null)
        {
            GrabPart(currentPartUnderHead);
        }
    }
    private void GrabPart(Part part)
    {
        if (part == null || _grabPosition == null)
            return;

        _holdingPartUI.SetHoldingPartUI(_armPosition, part.GetPartType());
        part.transform.SetParent(_grabPosition);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = Quaternion.identity;

        part.GetRigidbody().useGravity = false;
        part.GetRigidbody().isKinematic = true;
        
        SetCurrentGrabbedPart(part);
    }
    private void ReleasePart(Part part)
    {
        if (part == null)
            return;

        part.transform.SetParent(null);
        part.GetComponent<Rigidbody>().useGravity = true;
        part.GetComponent<Rigidbody>().isKinematic = false;
        
        SetCurrentGrabbedPart(null);
    }

    #endregion



        


}