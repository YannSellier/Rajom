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
    private HeadPickUp _currentHeadUnderHead;
    [SerializeField] private EInput changeHeadInput = EInput.ChangeHead1;
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
        UpdateHeadUnderHead();
        // PickNextHead(); plus utile
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
        if (_currentHeadUnderHead != null)
        {
            return _currentGrabbedHead.getHead();
        }
        return null;
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
        UpdateHeadUnderHead();
        if (_currentGrabbedHead != null && _currentHeadUnderHead != null)
        {
            SwitchHeadToHeadPickUp();
        } 
        else if (_currentGrabbedHead != null && _currentHeadUnderHead == null)
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
        if (_currentHeadUnderHead != null)
        {
            GrabCurrentHeadUnderHead();
            _currentHeadUnderHead = null;
        }
    }

    private void GrabCurrentHeadUnderHead()
    {
        if (_currentHeadUnderHead == null || _armGrabPosition == null)
            return;
        
        _currentGrabbedHead = _currentHeadUnderHead;
        RefreshArmHeadVisibility();
        _currentGrabbedHead.gameObject.SetActive(false);
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
    private void UpdateHeadUnderHead()
    {
        HeadPickUp headUnderHead = FindHeadUnderHead();
        
        if (headUnderHead != _currentHeadUnderHead)
            SetCurrentHeadUnderHead(headUnderHead);
    }
    
    private void SetCurrentHeadUnderHead(HeadPickUp headUnderHead)
    {
        _currentHeadUnderHead = headUnderHead;
    }
    
    private HeadPickUp FindHeadUnderHead()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("head");
        if (Physics.Raycast(transform.position, -transform.up, out hit, 100, mask))
        {
            HeadPickUp head = hit.collider.GetComponent<HeadPickUp>();
            return head;
        }

        return null;
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
