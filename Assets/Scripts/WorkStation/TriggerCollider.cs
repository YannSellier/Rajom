using System;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region COMPONENTS


    #endregion

    #region VARIABLES

    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerExit;

    #endregion

    #region GETTERS / SETTERS


    #endregion
    
    
    
    //=============================================================================
    // TRIGGER COLLIDER
    //=============================================================================

    #region TRIGGER COLLIDER

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }

    #endregion


}
