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
