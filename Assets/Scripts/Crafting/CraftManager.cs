public class CraftManager
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES


    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // CRAFTING
    //=============================================================================

    #region CRAFTING

    public bool CraftPart(Part part, Head head)
    {
        if (part == null || head == null)
            return false;

        PartModification modification = CreatePartModification(head);
        part.AddModification(modification);

        if (!ValidateCraftingResult(part))
            return false;
        
        return true;
    }
    
    private bool ValidateCraftingResult(Part resultPart)
    {
        return true;
    }
    private PartModification CreatePartModification(Head head)
    {
        return new PartModification(head.GetHeadType());
    }

    #endregion
        
}