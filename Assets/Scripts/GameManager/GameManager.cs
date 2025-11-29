using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region STATIC

    private static GameManager _instance;
    public static GameManager GetRef()
    {
        if (_instance == null)
            _instance = FindObjectOfType<GameManager>();
        if (_instance == null)
            Debug.LogError("GameManager: No instance found in scene!");
        
        return _instance;
    }
    
    #endregion

    #region VARIABLES

    private EGameState _gameState;
    
    // references to other managers
    private VacuumSpawner _vacuumSpawner;
    
    
    // events
    public Action<EGameState> onGameStateChanged;

    #endregion

    #region GETTERS / SETTERS

    // game state
    public EGameState GameState() => _gameState;
    private void SetGameState(EGameState newState)
    {
        _gameState = newState;
        
        onGameStateChanged?.Invoke(_gameState);
    }

    #endregion

    #region CONSTRUCTOR


    #endregion


    
    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        // get references to other managers
        _vacuumSpawner = FindObjectOfType<VacuumSpawner>();
    }
    protected void Start()
    {
        SetGameState(EGameState.MAIN_MENU);
    }

    #endregion



    //=============================================================================
    // GAME MANAGER
    //=============================================================================

    #region GAME STATES

    public void StartGame()
    {
        SetGameState(EGameState.IN_GAME);
        
        TriggerNextVacuum();
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
        TriggerNextVacuum();
    }
    public void TriggerNextVacuum()
    {
        _vacuumSpawner.SpawnAllParts();
    }

    #endregion


        
}