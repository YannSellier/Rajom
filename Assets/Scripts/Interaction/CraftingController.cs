using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingController : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private Part _partUnderHead = null;
    private HeadManager _headManager;
    
    private PlayerInput _playerInput;

    #endregion

    #region GETTERS / SETTERS

    private HeadManager GetHeadManager()
    {
        if (_headManager == null)
            _headManager = GetComponentInChildren<HeadManager>();
        return _headManager;
    }
    private Head GetCurrentHead() => GetHeadManager()?.GetCurrentHead();

    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {
        UpdatePartUnderHead();
        
        if (_playerInput.actions["Craft"].WasPerformedThisFrame())
        {
            TryCrafting();
        }
    }

    #endregion




    //=============================================================================
    // CRAFT CONTROLLER
    //=============================================================================

    #region INTERACTION

    public void TryCrafting()
    {
        if (GetCurrentHead() == null || _partUnderHead == null)
            return;

        CraftManager.GetRef().CraftPart(_partUnderHead, GetCurrentHead());
    }

    #endregion

    #region PART UNDER HEAD

    private Part GetPartUnderHead()
    {
        // Ray cast under
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Part");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, mask))
        {
            Part part = hit.collider.GetComponent<Part>();
            if (part != null)
                return part;
        }
        
        return null;
    }
    private void UpdatePartUnderHead()
    {
        Part partUnderHead = GetPartUnderHead();
        SetPartUnderHead(partUnderHead);
    }
    private void SetPartUnderHead(Part part)
    {
        if (_partUnderHead == part)
            return;
        
        if (_partUnderHead != null)
            _partUnderHead.OnHoverExit();
        
        _partUnderHead = part;
        if (_partUnderHead != null)
            _partUnderHead.OnHoverEnter();
    }

    #endregion


}
