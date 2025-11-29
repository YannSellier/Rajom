using UnityEngine;

public class PartSlot : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private BoxCollider _trigger;
    private Part _currentPart;
    
    private RecipesManager _recipesManager;

    [SerializeField] private Color _validPartColor = Color.green;
    [SerializeField] private Color _invalidPartColor = Color.red;

    #endregion

    #region GETTERS / SETTERS


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
    }
    
    // on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        // check if other is a part
        Part part = other.GetComponent<Part>();
        if (part != null)
            SetCurrentPart(part);
    }
    private void OnTriggerExit(Collider other)
    {
        // check if other is a part
        Part part = other.GetComponent<Part>();
        if (part != null && part == _currentPart)
            SetCurrentPart(null);
    }


    #endregion




    //=============================================================================
    // PART SLOT
    //=============================================================================

    #region PART SLOT

    private void SetCurrentPart(Part part)
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
        bool isValid = IsPartValidForSlot();
        Color targetColor = isValid ? _validPartColor : _invalidPartColor;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = targetColor;
    }
    private bool IsPartValidForSlot()
    {
        if (_currentPart == null)
            return false;

        Recipe currentRecipe = _recipesManager.GetCurrentRecipe();
        RecipeValidator recipeValidator = _recipesManager.GetRecipeValidator();
        return recipeValidator.IsPartFullyValidForRecipe(_currentPart, currentRecipe);
    }
    
    #endregion


        
}