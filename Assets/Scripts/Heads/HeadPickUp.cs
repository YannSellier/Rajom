using UnityEngine;

public class HeadPickUp : MonoBehaviour, IGrabbable
{
    #region VARIABLES
    
    [SerializeField] private GameObject _selectionVisualHead;
    [SerializeField] private Head _headToPickUp;
    private bool _isGrabbed = false;

    private bool _isPlayer1Hovering = false;
    private bool _isPlayer2Hovering = false;
    
    #endregion
    // Update is called once per frame
    
    #region GETTERS SETTERS

    public string GetName()
    {
        return "Head_" + getHead().GetHeadType();
    }
    public void SetIsGrabbed(bool newStateIsGrabbed)
    {
        _isGrabbed = newStateIsGrabbed;
    }
    public bool IsGrabbable()
    {
        return !_isGrabbed;
    }
    public Vector3 GetPosition() => transform.position;
    public Head getHead()
    {
        return _headToPickUp;
    }
    #endregion

    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        RefreshHoverStateVisual();
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
        _selectionVisualHead.SetActive(_isPlayer1Hovering || _isPlayer2Hovering);
    }
    
    #endregion
}
