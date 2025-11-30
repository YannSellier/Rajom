using UnityEngine;

public class PartSlot : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private EPartType _designatedPartType;
    private BoxCollider _trigger;
    private Part _currentPart;
    
    private RecipesManager _recipesManager;

    [SerializeField] private Color _validPartColor = Color.green;
    [SerializeField] private Color _invalidPartColor = Color.red;
    [SerializeField] private Color _emptyPartColor = Color.grey;

    #endregion

    #region GETTERS / SETTERS
    
    public Part GetCurrentPart() => _currentPart;
    public EPartType GetDesignatedPartType() => _designatedPartType;

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        _recipesManager = RecipesCreator.GetRef().GetRecipesesManager();
        _recipesManager.RegisterPartSlot(this);
    }
    

    #endregion




    //=============================================================================
    // PART SLOT
    //=============================================================================

    #region PART SLOT

    public void SetCurrentPart(Part part)
    {
        if (_currentPart == part)
            return;

        if (_currentPart != null)
            _currentPart.onPartCrafted -= RefreshVisuals;
        
        _currentPart = part;
        
        if (_currentPart != null)
            _currentPart.onPartCrafted += RefreshVisuals;
        
        OnPartChanged();
        
        Debug.Log("PartSlot: Current part set to " + (_currentPart != null ? _currentPart.name : "null"));
    }
    private void OnPartChanged()
    {
        _recipesManager.OnPartSlotChanged();
        
        RefreshVisuals();
    }

    #endregion

    #region VISUALS

    private void RefreshVisuals()
    {
        Color targetColor = GetSlotColor();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = targetColor;
    }
    private Color GetSlotColor()
    {
        if (_currentPart == null)
            return _emptyPartColor;

        return IsPartValidForSlot() ? _validPartColor : _invalidPartColor;
    }
    private bool IsPartValidForSlot()
    {
        if (_currentPart == null)
            return false;

        Recipe currentRecipe = _recipesManager.GetCurrentRecipe();
        RecipeValidator recipeValidator = _recipesManager.GetRecipeValidator();
        return recipeValidator.IsPartFullyValidForRecipe(_currentPart, currentRecipe, _designatedPartType);
    }
    
    #endregion


        
}