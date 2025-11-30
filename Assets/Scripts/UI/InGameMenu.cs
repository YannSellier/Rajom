using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenu : UIDisplayer
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    // ui refs
    private Label _timerLabel;
    
    [SerializeField] private VisualTreeAsset _recipePartAssets;
    
    private CurrentRecipeDisplayer _recipeDisplayer;
    private VisualElement _recipe_Root; 

    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region UI ELEMENT NAMES

    protected override string ROOT_NAME => "InGameMenu_Root";
    
    private const string TIMER_LABEL_NAME = "Timer_Label";
    private const string RECIPE_DISPLAYER_ROOT = "Recipe_Root" ;
    
    #endregion
    

    //=============================================================================
    // IN GAME MENU
    //=============================================================================

    #region SETUP

    protected override void Initialize()
    {
        base.Initialize();
        CreateRecipeDisplayer(RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe()); 
    }

    protected override void FindUIReferences()
    {
        base.FindUIReferences();
        
        _timerLabel = FindVisualElement<Label>(TIMER_LABEL_NAME);
        _recipe_Root = FindVisualElement<VisualElement>(RECIPE_DISPLAYER_ROOT);
       
    }
    
    protected override void BindListeners()
    {
        base.BindListeners();
        
        // other
        TimerManager.GetRef().onTimerUpdated += RefreshUI;
        GameManager.GetRef().onGameStateChanged += OnGameStateChanged;
        RecipesCreator.GetRef().GetRecipesesManager().onRecipeChange += RefreshUI;
    }
    
    protected override void UnbindListeners()
    {
        base.UnbindListeners();
        
        // other
        TimerManager.GetRef().onTimerUpdated -= RefreshUI;
        GameManager.GetRef().onGameStateChanged -= OnGameStateChanged;
        RecipesCreator.GetRef().GetRecipesesManager().onRecipeChange -= RefreshUI;
        
    }

    #endregion

    #region REFRESH UI

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        _timerLabel.text = GetTimerString();
    }
    private string GetTimerString()
    {
        float timeRemaining = TimerManager.GetRef().GetRemainingTime();
        
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    #endregion

    #region CALLBACKS

    private void OnGameStateChanged(EGameState newState)
    {
        if (newState == EGameState.IN_GAME)
            Open();
        else
            Close();
    }

    #endregion

    #region RECIPE

    public void CreateRecipeDisplayer(Recipe recipe)
    {
        _recipeDisplayer = new CurrentRecipeDisplayer(recipe, _recipe_Root, _recipePartAssets); 
        _recipeDisplayer.RefreshUI();
    }
    

    
    #endregion
}
