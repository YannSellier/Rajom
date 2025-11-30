using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ArmController;
using DefaultNamespace.UI;
using Unity.VisualScripting;

public class HeadManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    #region VARIABLES

    private PlayerInput playerInput;
    private HeadPickUp _currentGrabbedHead;
    [SerializeField] private EInput changeHeadInput = EInput.ChangeHead1;

    [SerializeField] private HoverController _hoverController;

        
    //test sans _grabPosition
    [SerializeField] private Transform _armGrabPosition;

    
    private List<Head> heads;
    

    #endregion

    
    
    #region START UPDATE

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        RefreshArmHeadVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerInput.actions[changeHeadInput.ToString()].WasPressedThisFrame())
        //    OnGrabInput();
        
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

    public HeadPickUp GetClosestHeadPickup() => _hoverController.GetClosestGrabbableOfType<HeadPickUp>();
    

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
        HeadPickUp closestHeadUnderHead = GetClosestHeadPickup();
        
        if (_currentGrabbedHead != null && closestHeadUnderHead != null)
        {
            SwitchHeadToHeadPickUp();
        } 
        else if (_currentGrabbedHead != null && closestHeadUnderHead == null)
        {
            ReleaseCurrentGrabbedHead();
        } 
        else
        {
            TryGrabbingHeadUnderHead(GetClosestHeadPickup());
        }
    }

    private void TryGrabbingHeadUnderHead(HeadPickUp closestHeadUnderHead)
    {
        if (closestHeadUnderHead != null)
        {
            GrabCurrentHeadUnderHead(closestHeadUnderHead);
        }
    }

    private void GrabCurrentHeadUnderHead(HeadPickUp closestHeadUnderHead)
    {
        if (closestHeadUnderHead == null || _armGrabPosition == null)
            return;
        
        SetCurrentGrabbedHead(closestHeadUnderHead);
    }
    private void SetCurrentGrabbedHead(HeadPickUp headPickup)
    {
        SoundManger _soundManager = FindObjectOfType<SoundManger>();
        _soundManager.PlaySfx(_soundManager.changeHead, 0.2f, false);
        
        if (_currentGrabbedHead != null)
        {
            _hoverController.RemoveGrabbableToIgnore(_currentGrabbedHead);
            _currentGrabbedHead.SetIsGrabbed(false);
        }

        _currentGrabbedHead = headPickup;
        
        FindObjectOfType<HoldingPartUI>().SetHoldingHeadUI(_hoverController.IsPlayer1() ? EArmPosition.LEFT : EArmPosition.RIGHT, _currentGrabbedHead);

        if (_currentGrabbedHead != null)
        {
            _hoverController.AddGrabbableToIgnore(headPickup);
            _currentGrabbedHead.gameObject.SetActive(false);
            _currentGrabbedHead.SetIsGrabbed(true);
        }

        RefreshArmHeadVisibility();
    }
    
    private void ReleaseCurrentGrabbedHead()
    {
        if (_currentGrabbedHead == null)
            return;
        // release current grabbed head at transform position of arm
        _currentGrabbedHead.transform.position = _armGrabPosition.position;
        _currentGrabbedHead.gameObject.SetActive(true);
        _currentGrabbedHead.GetComponent<Rigidbody>().useGravity = true;
        SetCurrentGrabbedHead(null);
        RefreshArmHeadVisibility();
        
    }

    #endregion
    
    
    
    #region HEAD UNDER HEAD

    #endregion

    #region SWITCH

    private void SwitchHeadToHeadPickUp()
    {
        HeadPickUp closestHeadUnderHead = GetClosestHeadPickup();
        ReleaseCurrentGrabbedHead();
        GrabCurrentHeadUnderHead(closestHeadUnderHead);
    }

    #endregion
}
