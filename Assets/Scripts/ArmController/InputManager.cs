using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private HeadManager _headManager;
    [SerializeField] private PartGrabController _partManager;
    [SerializeField] private EInput _input = EInput.Grab1;
    private PlayerInput _playerInput;
    
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    protected void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        _playerInput.actions[_input.ToString()].performed += ctx => ManageGrabHeadPart();
    }

    private void ManageGrabHeadPart()
    {
        if (_partManager.GetCurrentGrabbedPart() != null || _partManager.GetCurrentPartUnderHead() != null)
        {
            _partManager.callOnGrabInput();
        }
        else
        {
            _headManager.CallOnGrabInput();
        }
    }
}
