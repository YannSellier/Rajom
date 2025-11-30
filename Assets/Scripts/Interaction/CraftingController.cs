using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingController : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private HeadManager _headManager;
    
    private PlayerInput _playerInput;

    [SerializeField] private HoverController _hoverController;

    [SerializeField] private EInput _craftInput = EInput.Craft1;

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
        if (_playerInput.actions[_craftInput.ToString()].WasPerformedThisFrame())
        {
            TryCrafting();
        }
    }

    #endregion




    //=============================================================================
    // CRAFT CONTROLLER
    //=============================================================================

    #region INTERACTION

    public WorkStation GetWorkStationUnderHead()
    {
        return _hoverController.GetClosestGrabbableOfType<WorkStation>();
    }
    public void TryCrafting()
    {
        if (GetCurrentHead() == null ||  GetWorkStationUnderHead() == null)
            return;

        CraftManager.GetRef().CraftPart(GetWorkStationUnderHead().GetCurrentPartInTrigger(), GetWorkStationUnderHead(), GetCurrentHead());
    }

    #endregion

    #region PART UNDER HEAD

    private WorkStation GetStationUnderHead()
    {
        // Ray cast under
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("WorkStation");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, mask))
        {
            WorkStation part = hit.collider.GetComponentInChildren<WorkStation>();
            if (part == null)
                part = hit.collider.GetComponentInParent<WorkStation>();
            
            if (part != null)
                return part;
        }
        
        return null;
    }
    
    #endregion


}
