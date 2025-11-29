using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private float _remainTime;
    private bool _isRunning = false;
    
    // actions
    public Action onTimerUpdated;

    #endregion

    #region GETTERS / SETTERS

    public float GetRemainingTime() => _remainTime;
    public bool IsRunning() => _isRunning;

    #endregion

    #region STATIC

    private static TimerManager _instance; 
    public static TimerManager GetRef()
    {
        if (_instance == null)
            _instance = FindObjectOfType<TimerManager>();
        if (_instance == null)
            _instance = new GameObject("TimerManager").AddComponent<TimerManager>();
        return _instance;
    }

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    private void Update()
    {
        if (!_isRunning)
            return;
        
        UpdateTimer(Time.deltaTime);
    }

    #endregion




    //=============================================================================
    // TIMER
    //=============================================================================

    #region TIMER

    public void StartTimer(float duration)
    {
        SetRemainingTime(duration);
        _isRunning = true;
    }
    public void PauseTimer()
    {
        _isRunning = false;
    }
    public void ResumeTimer()
    {
        _isRunning = true;
    }
    private void UpdateTimer(float deltaTime)
    {
        if (!_isRunning)
            return;

        SetRemainingTime(_remainTime - deltaTime);
        if (_remainTime <= 0f)
            OnEndTimer();
    }
    private void OnEndTimer()
    {
        _isRunning = false;
        SetRemainingTime(0);
        
        GameManager.GetRef().LostGame();
    }
    private void SetRemainingTime(float time)
    {
        _remainTime = time;
        onTimerUpdated?.Invoke();
    }
    public void StopTimer()
    {
        _isRunning = false;
    }

    #endregion

        
}