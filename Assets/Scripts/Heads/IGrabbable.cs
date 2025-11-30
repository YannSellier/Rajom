using UnityEngine;

public interface IGrabbable
{
    //=============================================================================
    // GRABBABLE
    //=============================================================================

    #region GRABBABLE

    public void OnHoverEnter(bool isPlayer1);
    public void OnHoverExit(bool isPlayer1);
    public bool IsGrabbable();
    public Vector3 GetPosition();

    #endregion

}