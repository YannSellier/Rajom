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
        SoundManger _soundManager = FindObjectOfType<SoundManger>();
        _soundManager.PlaySfx(_soundManager.takePart, 0.5f, false);
        
        if (_currentGrabbedPart != null)
        {
            ReleasePart(_currentGrabbedPart);
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

        part.transform.SetParent(_grabPosition);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = Quaternion.identity;

        part.transform.position = GetGrabPosition(part);

        part.GetRigidbody().useGravity = false;
        part.GetRigidbody().isKinematic = true;
        
        SetCurrentGrabbedPart(part);
    }
    private Vector3 GetGrabPosition(Part currentPart)
    {
        Bounds bounds = GetObjectMeshBounds(currentPart.gameObject);

        Vector3 desiredGrabPos = _grabPosition.position;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;
        Vector3 offset = center - desiredGrabPos;

        return desiredGrabPos - offset;// - _grabPosition.forward * size.z;
    }
    
    public static Bounds GetObjectMeshBounds(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Cannot find bounds of null objet");
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        // Encapsulate all the mesh on this object and sub object
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if(renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);
        
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.bounds.size.AlmostEqual(Vector3.zero))
                continue;
            
            if (bounds.size.AlmostEqual(Vector3.zero))
                bounds.center = renderer.bounds.center;
            
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
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