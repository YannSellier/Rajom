using UnityEngine;

public class HeadPickUp : MonoBehaviour
{
    #region VARIABLES
    
    [SerializeField] private GameObject _selectionVisualHead;
    [SerializeField] private Head _headToPickUp;
    #endregion
    // Update is called once per frame
    
    #region GETTERS SETTERS

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
        OnHoverExit();
    }

    #endregion

    
    #region HOVER
    
    public void OnHoverEnter()
    {
        _selectionVisualHead.SetActive(true);
    }

    public void OnHoverExit()
    {
        _selectionVisualHead.SetActive(false);
    }
    #endregion
}
