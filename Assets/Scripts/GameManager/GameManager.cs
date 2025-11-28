using UnityEngine.InputSystem;

public class GameManager
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region STATIC

    private static GameManager _instance;
    public static GameManager GetRef()
    {
        if (_instance == null)
            _instance = new GameManager();
        
        return _instance;
    }
    
    #endregion

    #region VARIABLES

    private EGameState _gameState;

    #endregion

    #region GETTERS / SETTERS

    // game state
    public EGameState GameState() => _gameState;
    private void SetGameState(EGameState newState)
    {
        _gameState = newState;
    }

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // GAME MANAGER
    //=============================================================================

    #region GAME STATES

    public void StartGame()
    {
        SetGameState(EGameState.IN_GAME);
    }
    public void PauseGame()
    {
        SetGameState(EGameState.PAUSED);
    }
    public void ResumeGame()
    {
        SetGameState(EGameState.IN_GAME);
    }
    public void EndGame()
    {
        SetGameState(EGameState.GAME_OVER);
    }

    #endregion

    #region IN GAME

    public void OnVacuumComplete()
    {
        
    }
    public void TriggerNextVacuum()
    {
        
    }

    #endregion


        
}