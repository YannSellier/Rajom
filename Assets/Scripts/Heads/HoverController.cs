using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    private IGrabbable _closestGrabbableUnderGrabbable;
    private List<IGrabbable> _currentGrabbablesUnderGrabbable = new List<IGrabbable>();

    [SerializeField] private TriggerCollider _detectorTriggerCollider;

    [SerializeField] private bool _isPlayer1;
    
    private List<IGrabbable> _grabbablesToIgnore = new List<IGrabbable>();
    
    #endregion

    #region GETTERS / SETTERS

    public void AddGrabbableToIgnore(IGrabbable grabbableToIgnore)
    {
        if (_grabbablesToIgnore.Contains(grabbableToIgnore))
            return;
        _grabbablesToIgnore.Add(grabbableToIgnore);
        
        UpdateClosestGrabbable();
    }
    public void RemoveGrabbableToIgnore(IGrabbable grabbableToIgnore)
    {
        if (!_grabbablesToIgnore.Contains(grabbableToIgnore))
            return;
        
        _grabbablesToIgnore.Remove(grabbableToIgnore);
        
        UpdateClosestGrabbable();
    }
    public T GetClosestGrabbableOfType<T>() where T : IGrabbable
    {
        if (_closestGrabbableUnderGrabbable is T)
            return (T) _closestGrabbableUnderGrabbable;
        return default(T);
    }

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    private void Awake()
    {
        _detectorTriggerCollider.onTriggerEnter += OnDetectorTriggerEnter;
        _detectorTriggerCollider.onTriggerExit += OnDetectorTriggerExit;
    }

    #endregion



    //=============================================================================
    // HOVER
    //=============================================================================

    #region HOVER

    
    private void AddCurrentGrabbableUnderGrabbable(IGrabbable grabbableUnderGrabbable)
    {
        if (_currentGrabbablesUnderGrabbable.Contains(grabbableUnderGrabbable))
            return;
        
        _currentGrabbablesUnderGrabbable.Add(grabbableUnderGrabbable);
        UpdateClosestGrabbable();
    }
    public void RemoveGrabbableUnderGrabbable(IGrabbable grabbableUnderGrabbable)
    {
        if (!_currentGrabbablesUnderGrabbable.Contains(grabbableUnderGrabbable))
            return;

        if (_closestGrabbableUnderGrabbable == grabbableUnderGrabbable)
        {
            _closestGrabbableUnderGrabbable.OnHoverExit(_isPlayer1);
            _closestGrabbableUnderGrabbable = null;
        }

        _currentGrabbablesUnderGrabbable.Remove(grabbableUnderGrabbable);
        UpdateClosestGrabbable();
    }
    private void UpdateClosestGrabbable()
    {
        IGrabbable newClosest = GetClosestGrabbableUnderGrabbable();
        if (newClosest == _closestGrabbableUnderGrabbable)
            return;
        
        if (_closestGrabbableUnderGrabbable != null)
            _closestGrabbableUnderGrabbable.OnHoverExit(_isPlayer1);

        _closestGrabbableUnderGrabbable = newClosest;
        
        if (_closestGrabbableUnderGrabbable != null)
            _closestGrabbableUnderGrabbable.OnHoverEnter(_isPlayer1);
    }
    private IGrabbable GetClosestGrabbableUnderGrabbable()
    {
        IGrabbable newClosest = _closestGrabbableUnderGrabbable;
        float closestDistance = GetDistToClosestGrabbable(_closestGrabbableUnderGrabbable);
        foreach (IGrabbable grabbableUnderGrabbable in _currentGrabbablesUnderGrabbable)
        {
            if (_grabbablesToIgnore.Contains(grabbableUnderGrabbable) || !grabbableUnderGrabbable.IsGrabbable())
                continue;
            
            float newDist = GetDistToClosestGrabbable(grabbableUnderGrabbable);
            if (newDist < closestDistance)
            {
                closestDistance = newDist;
                newClosest = grabbableUnderGrabbable;
            }
        }

        return newClosest;
    }
    private float GetDistToClosestGrabbable(IGrabbable grabbable)
    {
        if (grabbable == null)
            return float.MaxValue;
        return Vector3.Distance(grabbable.GetPosition(), transform.position);
    }
    
    private void OnDetectorTriggerEnter(Collider other)
    {
        if (other == null)
            return;
        
        IGrabbable grabbable = other.GetComponentInParent<IGrabbable>();
        if (grabbable == null)
            return;
        
        AddCurrentGrabbableUnderGrabbable(grabbable);
    }
    private void OnDetectorTriggerExit(Collider other)
    {
        if (other == null)
            return;
        
        IGrabbable grabbable = other.GetComponentInParent<IGrabbable>();
        if (grabbable == null)
            return;
        
        RemoveGrabbableUnderGrabbable(grabbable);
    }
    

    #endregion

        
}