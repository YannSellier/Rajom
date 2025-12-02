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

    [SerializeField] private PartGrabController _grabController1;
    [SerializeField] private PartGrabController _grabController2;
    [SerializeField] private HeadManager _headManager1;
    [SerializeField] private HeadManager _headManager2;
    
    private CurrentRecipeDisplayer _recipeDisplayer;
    private VisualElement _recipe_Root; 

    private VisualElement _holdingPartUILeft;
    private VisualElement _holdingPartUIRight;
    private VisualElement _holdingHeadUILeft;
    private VisualElement _holdingHeadUIRight;
    
    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region UI ELEMENT NAMES

    protected override string ROOT_NAME => "InGameMenu_Root";
    
    private const string TIMER_LABEL_NAME = "Timer_Label";
    private const string RECIPE_DISPLAYER_ROOT = "Recipe_Root" ;
    
    private const string PART_HOLDER_VISUAL_LEFT_NAME = "LeftPart_Holder";
    private const string HEAD_HOLDER_VISUAL_LEFT_NAME = "LeftHead_Holder";
    private const string PART_HOLDER_VISUAL_RIGHT_NAME = "RightPart_Holder";
    private const string HEAD_HOLDER_VISUAL_RIGHT_NAME = "RightHead_Holder";
    
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
       
        _holdingPartUILeft = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_LEFT_NAME);
        _holdingHeadUILeft = FindVisualElement<VisualElement>(HEAD_HOLDER_VISUAL_LEFT_NAME);
        _holdingPartUIRight = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_RIGHT_NAME);
        _holdingHeadUIRight = FindVisualElement<VisualElement>(HEAD_HOLDER_VISUAL_RIGHT_NAME);
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
        
        CreateRecipeDisplayer(RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe());
        
        RefreshHold();

    }
    private string GetTimerString()
    {
        float timeRemaining = TimerManager.GetRef().GetRemainingTime();
        
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void RefreshHold()
    {
        RefreshPartHold(_grabController1, _holdingPartUILeft);
        RefreshPartHold(_grabController2, _holdingPartUIRight);
        RefreshHeadHold(_headManager1, _holdingHeadUILeft);
        RefreshHeadHold(_headManager2, _holdingHeadUIRight);
    }
    private void RefreshHeadHold(HeadManager headManager, VisualElement display)
    {
        Head head = headManager.GetCurrentHead();
        
        display.style.display = head == null ? DisplayStyle.None : DisplayStyle.Flex;
        if (head == null)
            return;

        ResetClasses(display);
        display.AddToClassList(GetCssForHeadType(head.GetHeadType()));
    }
    private void RefreshPartHold(PartGrabController grabController, VisualElement display)
    {
        Part part = grabController.GetCurrentGrabbedPart();
        
        display.style.display = part == null ? DisplayStyle.None : DisplayStyle.Flex;
        if (part == null)
            return;

        ResetClasses(display);
        display.AddToClassList(GetCssForPartType(part.GetPartType()));
    }

    private void ResetClasses(VisualElement display)
    {
        display.RemoveFromClassList(GetCssForPartType(EPartType.HANDLE));
        display.RemoveFromClassList(GetCssForPartType(EPartType.ALIM));
        display.RemoveFromClassList(GetCssForPartType(EPartType.PIPE));
        display.RemoveFromClassList(GetCssForPartType(EPartType.HEAD));
        
        display.RemoveFromClassList(GetCssForHeadType(EHeadType.HAMMER));
        display.RemoveFromClassList(GetCssForHeadType(EHeadType.SCREW));
        display.RemoveFromClassList(GetCssForHeadType(EHeadType.SAW));
        display.RemoveFromClassList(GetCssForHeadType(EHeadType.PLIERS));
    }
    public string GetCssForHeadType(EHeadType type)
    {
        return type switch
        {
            EHeadType.HAMMER => "hammer_icon",
            EHeadType.SCREW => "drill_icon",
            EHeadType.SAW => "saw_icon",
            EHeadType.PLIERS => "welder_icon",
            _ => string.Empty
        };
    }
    
    public string GetCssForPartType(EPartType type)
    {
        return type switch
        {
            EPartType.HANDLE => "handle_part",
            EPartType.ALIM => "alim_part",
            EPartType.HEAD => "head_part",
            EPartType.PIPE => "pipe_part",
            _ => string.Empty
        };
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
