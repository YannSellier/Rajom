using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;

public class HeadManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    #region VARIABLES

    private PlayerInput playerInput;
    private HeadPickUp _currentGrabbedHead;
    private HeadPickUp _closestHeadUnderHead;
    private List<HeadPickUp> _currentHeadsUnderHead = new List<HeadPickUp>();
    [SerializeField] private EInput changeHeadInput = EInput.ChangeHead1;

        
    //test sans _grabPosition
    [SerializeField] private Transform _armGrabPosition;

    [SerializeField] private TriggerCollider _detectorTriggerCollider;
    private TriggerStack<HeadPickUp> _triggerStack;
    
    private List<Head> heads;
    

    #endregion

    
    
    #region START UPDATE

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        _detectorTriggerCollider.onTriggerEnter += OnDetectorTriggerEnter;
        _detectorTriggerCollider.onTriggerExit += OnDetectorTriggerExit;
    }
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        RefreshArmHeadVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClosestHead();
        
        if (playerInput.actions[changeHeadInput.ToString()].WasPressedThisFrame())
            OnGrabInput();
        
    }

    #endregion

    #region CHECK

    
    /**
     * Peut être ne pas utiliser !!
     */

    #endregion
    
    #region GETTERS

    public Head GetCurrentHead()
    {
        return _currentGrabbedHead?.getHead();
    }
    

    #endregion
    
    
    #region  GRAB INPUT RELEASE
    
    //Refresh can set active head on the arm
    public void RefreshArmHeadVisibility()
    {
        foreach (Head h in heads)
        {
            bool shouldBeActive = false;
            if (_currentGrabbedHead !=null)
            {
                Head head = _currentGrabbedHead.getHead();
                shouldBeActive = _currentGrabbedHead != null && h.GetHeadType() == head.GetHeadType();
            }
            h.gameObject.SetActive(shouldBeActive);
        }
    }


    public void CallOnGrabInput()
    {
        OnGrabInput();
    }
    /// <summary>
    /// Quand l'input est appuyé peut :
    /// switch la head tenu avec celle en dessous
    /// Poser la head maintenue
    /// Recupérer une head
    /// </summary>
    private void OnGrabInput()
    {
        if (_currentGrabbedHead != null && _closestHeadUnderHead != null)
        {
            SwitchHeadToHeadPickUp();
        } 
        else if (_currentGrabbedHead != null && _closestHeadUnderHead == null)
        {
            ReleaseCurrentGrabbedHead();
        } 
        else
        {
            TryGrabbingHeadUnderHead();
        }
    }

    private void TryGrabbingHeadUnderHead()
    {
        if (_closestHeadUnderHead != null)
        {
            GrabCurrentHeadUnderHead();
        }
    }

    private void GrabCurrentHeadUnderHead()
    {
        if (_closestHeadUnderHead == null || _armGrabPosition == null)
            return;
        
        _currentGrabbedHead = _closestHeadUnderHead;
        RefreshArmHeadVisibility();
        _currentGrabbedHead.gameObject.SetActive(false);
        _closestHeadUnderHead = null;
        UpdateClosestHead();
    }
    
    private void ReleaseCurrentGrabbedHead()
    {
        if (_currentGrabbedHead == null)
            return;
        // release current grabbed head at transform position of arm
        _currentGrabbedHead.transform.position = _armGrabPosition.position;
        _currentGrabbedHead.gameObject.SetActive(true);
        _currentGrabbedHead.GetComponent<Rigidbody>().useGravity = true;
        _currentGrabbedHead = null;
        RefreshArmHeadVisibility();
        
    }

    #endregion
    
    
    
    #region HEAD UNDER HEAD
    
    private void AddCurrentHeadUnderHead(HeadPickUp headUnderHead)
    {
        if (_currentHeadsUnderHead.Contains(headUnderHead))
            return;
        
        _currentHeadsUnderHead.Add(headUnderHead);
        UpdateClosestHead();
    }
    private void RemoveHeadUnderHead(HeadPickUp headUnderHead)
    {
        if (!_currentHeadsUnderHead.Contains(headUnderHead))
            return;

        if (_closestHeadUnderHead == headUnderHead)
        {
            _closestHeadUnderHead.OnHoverExit();
            _closestHeadUnderHead = null;
        }

        _currentHeadsUnderHead.Remove(headUnderHead);
        UpdateClosestHead();
    }
    private void UpdateClosestHead()
    {
        HeadPickUp newClosest = GetClosestHeadUnderHead();
        if (newClosest == _closestHeadUnderHead)
            return;
        
        if (_closestHeadUnderHead != null)
            _closestHeadUnderHead.OnHoverExit();

        _closestHeadUnderHead = newClosest;
        
        if (_closestHeadUnderHead != null)
            _closestHeadUnderHead.OnHoverEnter();
    }
    private HeadPickUp GetClosestHeadUnderHead()
    {
        HeadPickUp newClosest = _closestHeadUnderHead;
        float closestDistance = GetDistToClosestHead(_closestHeadUnderHead);
        foreach (HeadPickUp headUnderHead in _currentHeadsUnderHead)
        {
            if (headUnderHead == _currentGrabbedHead)
                continue;
            
            float newDist = GetDistToClosestHead(headUnderHead);
            if (newDist < closestDistance)
            {
                closestDistance = newDist;
                newClosest = headUnderHead;
            }
        }

        return newClosest;
    }
    private float GetDistToClosestHead(HeadPickUp head)
    {
        if (head == null)
            return float.MaxValue;
        return Vector3.Distance(head.transform.position, _armGrabPosition.position);
    }
    
    private void OnDetectorTriggerEnter(Collider other)
    {
        if (other == null)
            return;
        
        HeadPickUp head = other.GetComponent<HeadPickUp>();
        if (head == null)
            return;
        
        AddCurrentHeadUnderHead(head);
    }
    private void OnDetectorTriggerExit(Collider other)
    {
        if (other == null)
            return;
        
        HeadPickUp head = other.GetComponent<HeadPickUp>();
        if (head == null)
            return;
        
        RemoveHeadUnderHead(head);
    }
    
    #endregion

    #region SWITCH

    private void SwitchHeadToHeadPickUp()
    {
        ReleaseCurrentGrabbedHead();
        GrabCurrentHeadUnderHead();
    }

    #endregion
}
