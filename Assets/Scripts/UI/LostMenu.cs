using UnityEngine;
using UnityEngine.UIElements;

public class LostMenu : UIDisplayer
{
    #region VARIABLES
    
    //ui references
    private Button _retryButton;
    private Button _mainMenuButton;
    
    #endregion
    
    #region UI ELEMENT NAMES

    private const string LOST_MENU_ROOT_NAME = "Lost_Menu_Root";
    private const string RETRY_BUTTON_NAME = "Retry_Button";
    private const string MAIN_MENU_BUTTON = "Main_Menu_Button";
    
    protected override string ROOT_NAME => LOST_MENU_ROOT_NAME;
    
    #endregion
    
    #region LOST MENU

    protected override void FindUIReferences()
    {
        base.FindUIReferences();
        
        _retryButton = FindVisualElement<Button>(RETRY_BUTTON_NAME);
        _mainMenuButton = FindVisualElement<Button>(MAIN_MENU_BUTTON);
    }

    protected override void BindListeners()
    {
        base.BindListeners();

        _retryButton.clicked += OnRetryButtonClicked;
        _mainMenuButton.clicked += OnMainMenuButtonClicked;

        GameManager.GetRef().onGameStateChanged += OnGameStateChanged;
    }
    
    #endregion
    
    #region CALLBACKS

    private void OnRetryButtonClicked()
    {
        
    }

    private void OnMainMenuButtonClicked()
    {
        
    }

    private void OnGameStateChanged(EGameState newState)
    {
        if (newState == EGameState.GAME_OVER)
            Open();
        else
            Close();
    }
    #endregion
}
