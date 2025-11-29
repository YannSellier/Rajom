using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : UIDisplayer
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    
    // ui references
    private Button _startButton;
    private Button _quitButton;

    #endregion

    #region UI ELEMENT NAMES

    private const string MAIN_MENU_ROOT_NAME = "MainMenu_Root";
    private const string START_BUTTON_NAME = "Start_Button";
    private const string QUIT_BUTTON_NAME = "Quit_Button";

    protected override string ROOT_NAME => MAIN_MENU_ROOT_NAME;
    
    #endregion

    #region GETTERS / SETTERS


    #endregion



    //=============================================================================
    // MAIN MENU
    //=============================================================================

    #region MAIN MENU

    protected override void FindUIReferences()
    {
        base.FindUIReferences();

        _startButton = FindVisualElement<Button>(START_BUTTON_NAME);
        _quitButton = FindVisualElement<Button>(QUIT_BUTTON_NAME);
    }
    protected override void BindListeners()
    {
        base.BindListeners();

        // ui
        _startButton.clicked += OnStartButtonClicked;
        _quitButton.clicked += OnQuitButtonClicked;
        
        // other
        GameManager.GetRef().onGameStateChanged += OnGameStateChanged;
    }

    #endregion

    #region REFRESH UI


    #endregion

    #region CALLBACKS

    // UI
    private void OnStartButtonClicked()
    {
        GameManager.GetRef().StartGame();
    }
    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit Button Clicked - Quitting Game...");
        Application.Quit();
    }

    // Game Manager
    private void OnGameStateChanged(EGameState newState)
    {
        if (newState == EGameState.MAIN_MENU)
            Open();
        else
            Close();
    }
    
    #endregion



}
