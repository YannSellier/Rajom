using UnityEngine;

[System.Serializable]
public class PartModification
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private EHeadType _headType;

    #endregion

    #region GETTERS / SETTERS

    public EHeadType GetHeadType() => _headType;

    #endregion

    #region CONSTRUCTOR

    public PartModification(EHeadType headType)
    {
        _headType = headType;
    }

    #endregion
    
    
}