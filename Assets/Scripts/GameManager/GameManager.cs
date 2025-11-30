using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float _gameDuration = 120f;
    [SerializeField] private float _delayBetweenVacuums = 2f;

    [SerializeField] private GameObject[] _tutorialObjects;
    private int _tutorialStep = -1;
    
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
    
    public EGameState GetGameState() => _gameState;

    public List<Part> GetParts() => new List<Part>(GameObject.FindObjectsOfType<Part>());
        
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
        MainMenu();
    }

    protected void Update()
    {
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput.actions["Enter"].WasPerformedThisFrame())
            TutorialInput();
    }

    #endregion



    //=============================================================================
    // GAME MANAGER
    //=============================================================================

    #region GAME STATES

    public void StartGame()
    {
        StartTutorial();
    }
    public void ReallyStartGame()
    {
        SetGameState(EGameState.IN_GAME);
        
        TriggerNextVacuum();
    }
    public void PauseGame()
    {
        SetGameState(EGameState.PAUSED);
        
        TimerManager.GetRef().PauseTimer();
    }
    public void ResumeGame()
    {
        SetGameState(EGameState.IN_GAME);
        
        TimerManager.GetRef().ResumeTimer();
    }
    public void LostGame()
    {
        SetGameState(EGameState.GAME_OVER);
        
        TimerManager.GetRef().StopTimer();
    }
    public void EndGame()
    {
        SetGameState(EGameState.GAME_OVER);
        
        TimerManager.GetRef().StopTimer();
    }

    public void MainMenu()
    {
        SetGameState(EGameState.MAIN_MENU);
    }

    #endregion

    #region IN GAME

    public void OnVacuumComplete()
    {
        StartCoroutine(TriggerNextVacuumAfterDelay());
        
        TimerManager.GetRef().StopTimer();
    }
    public IEnumerator TriggerNextVacuumAfterDelay()
    {
        yield return new WaitForSeconds(_delayBetweenVacuums);
        
        TriggerNextVacuum();
    }
    public void TriggerNextVacuum()
    {
        RecipesCreator.GetRef().GetRecipesesManager().PickNextRecipe();
        
        _vacuumSpawner.SpawnAllParts();
        
        TimerManager.GetRef().StartTimer(_gameDuration);

    }

    public void VacuumCleaner()
    {
        List<Part> listePart = GetParts();
        foreach (Part partToDestroy in listePart)
        {
            Destroy(partToDestroy.gameObject);
        }
    }

    #endregion

    #region TUTORIAL

    public void StartTutorial()
    {
        _tutorialStep = 0;
        RefreshTutorialVisuals();
    }

    public void TutorialInput()
    {
        if (_tutorialStep == -1)
            return;
        
        _tutorialStep++;
        if (_tutorialStep >= _tutorialObjects.Length)
            EndTutorial();
        RefreshTutorialVisuals();
    }
    public void EndTutorial()
    {
        _tutorialStep = -1;
        ReallyStartGame();
    }

    public void RefreshTutorialVisuals()
    {
        for (int i = 0; i < _tutorialObjects.Length; i++)
        {
            _tutorialObjects[i].SetActive(_tutorialStep == i);
        }
    }

    #endregion


    

        
}