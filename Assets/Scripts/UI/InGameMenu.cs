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

    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region UI ELEMENT NAMES

    protected override string ROOT_NAME => "InGameMenu_Root";
    
    private const string TIMER_LABEL_NAME = "Timer_Label";

    [SerializeField] private VisualTreeAsset _recipe_asset; 
    [SerializeField] private VisualTreeAsset _recipe_part_asset; 
    [SerializeField] private VisualTreeAsset _recipe_part_modifications_asset; 
    
    private const string RECIPE_CONTAINER_NAME = "Recipe_Root";
    private const string RECIPE_PART_CONTAINER_NAME = "Recipe_parts";
    private const string RECIPE_PART_MODIFIACTION_CONTAINER_NAME = "Recipe_part_modifications";
    
    private VisualElement _recipesContainer;
    private VisualElement _recipesPartsContainer;
    private VisualElement _recipesPartModificationsContainer;
    
    #endregion



    //=============================================================================
    // IN GAME MENU
    //=============================================================================

    #region SETUP

    protected override void FindUIReferences()
    {
        base.FindUIReferences();
        
        _timerLabel = FindVisualElement<Label>(TIMER_LABEL_NAME);
        _recipesContainer = FindVisualElement<VisualElement>(RECIPE_CONTAINER_NAME);
        _recipesPartsContainer = FindVisualElement<VisualElement>(RECIPE_PART_CONTAINER_NAME);
        _recipesPartModificationsContainer = FindVisualElement<VisualElement>(RECIPE_PART_MODIFIACTION_CONTAINER_NAME);
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
         
        CreateRecipe();
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

    #region BUILD RECIPE

    private void CreateRecipe()
    {
        Recipe currentRecipe = RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe();
        string recipeElementId = $"recipe-{currentRecipe.GetName()}";

        VisualElement existing = _recipesContainer.Q<VisualElement>(recipeElementId);
        if (existing != null)
            return;
        
        VisualElement newRecipeElement = _recipe_asset.Instantiate();
        newRecipeElement.name = recipeElementId;

        newRecipeElement.Q<Label>("Recipe_name").text = currentRecipe.GetName();
        newRecipeElement.Q<Label>("Recipe_description").text = currentRecipe.GetDescription();
        
        foreach (PartData partData in currentRecipe.GetParts())
        {
            VisualElement partElement = _recipe_part_asset.CloneTree();

            Label recipePartTypeElement = partElement.Q<Label>("Recipe_part_type");
            if (recipePartTypeElement != null)
                recipePartTypeElement.text = partData.GetPartType().ToString();
            
            foreach (PartModification modification in partData.GetModifications())
            {
                VisualElement modificationElement = _recipe_part_modifications_asset.CloneTree();

                Label modificationLabel = modificationElement.Q<Label>("Recipe_part_modification");
                if (modificationLabel != null)
                    modificationLabel.text = modification.GetHeadType().ToString();

                _recipesPartModificationsContainer.Add(modificationElement);
            }

            _recipesPartsContainer.Add(partElement);
        }

        _recipesContainer.Add(newRecipeElement);
    }
    #endregion
}
