using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.UI
{
    public class PauseButton : UIDisplayer
    {
        #region VARIABLES

        private Button _pauseButton;
        
        #endregion
        
        #region UI ELEMENT NAMES

        private const string PAUSE_BUTTON_NAME = "Pause_Button";
        private const string PAUSE_BUTTON_ROOT_NAME = "Pause_Button_Root";
        
        protected override string ROOT_NAME => PAUSE_BUTTON_ROOT_NAME;
        
        #endregion
        
        #region PAUSE BUTTON
        protected override void FindUIReferences()
        {
            base.FindUIReferences();
            
            _pauseButton = FindVisualElement<Button>(PAUSE_BUTTON_NAME);
        }

        protected override void BindListeners()
        {
            base.BindListeners();

            _pauseButton.clicked += OnPauseButtonClicked;

            GameManager.GetRef().onGameStateChanged += OnGameStateChanged;
        }

        #endregion
        
        
        #region CALLBACKS

        private void OnPauseButtonClicked()
        {
            GameManager.GetRef().PauseGame();
        }

        private void OnGameStateChanged(EGameState newState)
        {
            if (newState == EGameState.IN_GAME)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
        
        #endregion
    }
}