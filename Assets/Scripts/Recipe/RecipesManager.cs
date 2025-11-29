using System;
using System.Collections.Generic;
using NUnit.Framework;

public class RecipesManager
{
    //=============================================================================
    // VARIABLES
    //=============================================================================
    
    #region VARIABLES

    public Action onRecipeChange;  
    private List<Recipe> _recipes;
    private RecipeValidator _recipeValidator = new RecipeValidator();
    private int _currentRecipeIndex = -1;
    
    // All Part in the scene 
    private List<PartSlot> _currentPartsSlots = new List<PartSlot>();
        
    
    #endregion

    #region GETTERS / SETTERS


    public RecipeValidator GetRecipeValidator() => _recipeValidator;
    public Recipe GetCurrentRecipe()
    {
        if (_recipes.Count == 0)
            return null;

        if (_currentRecipeIndex == -1)
            _currentRecipeIndex = 0;

        return _recipes[_currentRecipeIndex];
    }
    public void PickNextRecipe()
    {
        _currentRecipeIndex = (_currentRecipeIndex + 1) % _recipes.Count;
        onRecipeChange?.Invoke();
    }
    public List<PartSlot> GetPartsSlots() => _currentPartsSlots;
    
    #endregion
    


    //=============================================================================
    // CONSTRUCTOR
    //=============================================================================

    public RecipesManager(List<Recipe> recipes)
    { 
        _recipes = recipes ?? new List<Recipe>();
    }
    
    //=============================================================================
    // GETTERS / SETTERS
    //=============================================================================
    
    #region GETTERS / SETTERS

    public List<Recipe> GetRecipes() => _recipes; 
    
    public void PushRecipe(Recipe recipe)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));

        _recipes.Add(recipe);
    }

    public void RegisterPartSlot(PartSlot partSlot)
    {
        _currentPartsSlots.Add(partSlot);
    }
    #endregion



    //=============================================================================
    // PART SLOTS
    //=============================================================================

    #region PART SLOTS

    public void OnPartSlotChanged()
    {
        Recipe currentRecipe = GetCurrentRecipe(); 
        if (currentRecipe.GetParts().Count != _currentPartsSlots.Count)
        {
            return;
        }

        for (int i = 0; i < _currentPartsSlots.Count ; i++)
        {
            PartSlot slot = _currentPartsSlots[i];
                
            if (!_recipeValidator.IsPartFullyValidForRecipe(slot.GetCurrentPart(), currentRecipe, slot.GetDesignatedPartType()))
            {
                return; 
            }
        }
        
        // VacuumAssembler.GetRef().AssembleVacuum();
    }

    #endregion

}
