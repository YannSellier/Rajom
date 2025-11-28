using System;
using UnityEngine;

public class Part : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private EPartType _headType;

    public Action<EPartType> onPartDeleted;

    #endregion

    #region GETTERS / SETTERS

    public EPartType GetPartType() => _headType;

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // PART
    //=============================================================================

    #region PART

    public void Delete()
    {
        onPartDeleted?.Invoke(_headType);
        
        Destroy(gameObject);
    }

    #endregion
        
}