using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.UI
{
    public class PauseMenu : UIDisplayer
    {
        #region VARIABLES

        private Button _resumeButton;
        private Button _restartButton;
        private Button _giveUpButton;
        
        #endregion
        
        #region UI ELEMENT NAMES

        private const string PAUSE_MENU_ROOT_NAME = "Pause_Menu_Root";
        private const string RESUME_BUTTON_NAME = "Resume_Button";
        private const string RESTART_BUTTON_NAME =  "Restart_Button";
        private const string GIVE_UP_BUTTON_NAME = "Give_Up_Button";

        protected override string ROOT_NAME => PAUSE_MENU_ROOT_NAME;

        #endregion

        #region PAUSE MENU

        protected override void FindUIReferences()
        {
            base.FindUIReferences();
            
            _resumeButton = FindVisualElement<Button>(RESUME_BUTTON_NAME);
            _restartButton = FindVisualElement<Button>(RESTART_BUTTON_NAME);
            _giveUpButton = FindVisualElement<Button>(GIVE_UP_BUTTON_NAME);
            
        }

        protected override void BindListeners()
        {
            base.BindListeners();

            _resumeButton.clicked += OnResumeButtonClicked;
            _restartButton.clicked += OnRestartButtonClicked;
            _giveUpButton.clicked += OnGiveUpButtonClicked;

            GameManager.GetRef().onGameStateChanged += OnGameStateChanged;
        }

        #endregion
        
        #region CALLBACKS

        private void OnResumeButtonClicked()
        {
            GameManager.GetRef().ResumeGame();
        }
        private void OnRestartButtonClicked()
        {
            GameManager.GetRef().VacuumCleaner();
            GameManager.GetRef().StartGame();
        }
        private void OnGiveUpButtonClicked()
        {
            GameManager.GetRef().MainMenu();
        }


        private void OnGameStateChanged(EGameState newState)
        {
            if (newState == EGameState.PAUSED)
                Open();
            else
                Close();
        }
        #endregion
        
        
    }
}