using UnityEngine;

[System.Serializable]
public class PartModification
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private EHeadType _headType;
    [SerializeField] private EWorkStationType _workStationType;

    #endregion

    #region GETTERS / SETTERS

    public EHeadType GetHeadType() => _headType;
    public EWorkStationType GetWorkStationType() => _workStationType;

    #endregion

    #region CONSTRUCTOR

    public PartModification(EHeadType headType, EWorkStationType workStationType)
    {
        _headType = headType;
        _workStationType = workStationType;
    }

    #endregion
    
    
}