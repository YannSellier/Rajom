using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerStack<T> where T : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private TriggerCollider _triggerCollider;
    private List<T> _stack;
    private T _currentObj;

    public Action<T> onCurrentObjChanged;

    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region CONSTRUCTOR

    public TriggerStack(TriggerCollider triggerCollider)
    {
        _stack = new List<T>();
        _triggerCollider = triggerCollider;

        _triggerCollider.onTriggerEnter += OnDetectorTriggerEnter;
        _triggerCollider.onTriggerExit += OnDetectorTriggerExit;
    }

    #endregion



    //=============================================================================
    // TRIGGER
    //=============================================================================

    #region TRIGGER

    private void SetCurrentObj(T currentObj)
    {
        if (_currentObj != null)
            _stack.Add(_currentObj);
        
        _currentObj = currentObj;
        
        onCurrentObjChanged?.Invoke(currentObj);
    }
    public void UnsetCurrentObj(T currentObj)
    {
        if (currentObj != _currentObj)
        {
            _stack.Remove(_currentObj);
            return;
        }

        _currentObj = _stack.Count > 0 ? _stack.Last() : null;
        
        if (_currentObj != null)
            _stack.Remove(_currentObj);
        
        onCurrentObjChanged?.Invoke(_currentObj);
    }
    private void OnDetectorTriggerEnter(Collider other)
    {
        if (other == null)
            return;
        
        T obj = other.GetComponent<T>();
        if (obj == null)
            return;
        
        SetCurrentObj(obj);
    }
    private void OnDetectorTriggerExit(Collider other)
    {
        if (other == null)
            return;
        
        T obj = other.GetComponent<T>();
        if (obj == null)
            return;
        
        UnsetCurrentObj(obj);
    }


    #endregion

        
}