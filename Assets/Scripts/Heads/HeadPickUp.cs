using UnityEngine;

public class HeadPickUp : MonoBehaviour
{
    #region VARIABLES
    
    [SerializeField] private GameObject _selectionVisualHead;
    
    #endregion
    // Update is called once per frame
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
