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
    
    private PlayerInput _playerInput;
    private Part _currentGrabbedPart;
    private Part _currentPartUnderHead;
    
    #endregion

    #region GETTERS / SETTERS

    

    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.actions[_grabInput.ToString()].performed += ctx => OnGrabInput();
    }
    protected void Update()
    {
        UpdatePartUnderHead();
    }

    #endregion



    //=============================================================================
    // GRAB CONTROLLER
    //=============================================================================

    #region GRAB CONTROLLER

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

    private void UpdatePartUnderHead()
    {
        Part partUnderHead = FindPartUnderHead();
        if (partUnderHead != _currentPartUnderHead)
            SetCurrentPartUnderHead(partUnderHead);
    }
    private void SetCurrentPartUnderHead(Part part)
    {
        if (_currentPartUnderHead != null)
            _currentPartUnderHead.OnHoverExit();
        
        _currentPartUnderHead = part;
        
        if (_currentPartUnderHead != null)
            _currentPartUnderHead.OnHoverEnter();
    }
    private Part FindPartUnderHead()
    {
        // Raycast to find part under head
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Part");
        if (Physics.Raycast(transform.position, -transform.up, out hit, 100, mask))
        {
            Part part = hit.collider.GetComponent<Part>();
            return part;
        }

        return null;
    }

    #endregion

        


}