using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
using DefaultNamespace;

public class HeadManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private PlayerInput playerInput;
    private Head _currentGrabbedHead;
    private Head _currentHeadUnderHead = null;
    [SerializeField] private EInput changeHeadInput = EInput.ChangeHead1;
    //test sans _grabPosition
    [SerializeField] private Transform _armGrabPosition;
    
    private List<Head> heads;
    
    
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        RefreshArmHeadVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions[changeHeadInput.ToString()].WasPressedThisFrame())
            OnGrabInput();
            
        UpdateHeadUnderHead();
            // PickNextHead(); plus utile
    }


    public Head GetCurrentHead()
    {
        return _currentGrabbedHead;
    }
    
    //Refresh can set active head on the arm
    public void RefreshArmHeadVisibility()
    {
        foreach (Head h in heads)
        {
            bool shouldBeActive = _currentGrabbedHead != null && h.GetHeadType() == _currentGrabbedHead.GetHeadType();
            h.gameObject.SetActive(shouldBeActive);
        }
    }


    #region  GRAB INPUT RELEASE
    
    private void OnGrabInput()
    {
        if (_currentGrabbedHead != null)
        {
            //drop la head
            ReleaseCurrentGrabbedHead();
        }
        else
        {
            //Debug.Log("try Grabbing");
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
        _currentGrabbedHead = null;
        RefreshArmHeadVisibility();
        
    }

    #endregion
    
    
    
    
    
    
    #region HEAD UNDER HEAD
    private void UpdateHeadUnderHead()
    {
        Head headUnderHead = FindHeadUnderHead();
        
        if (headUnderHead != _currentHeadUnderHead)
            SetCurrentHeadUnderHead(headUnderHead);
    }
    
    private void SetCurrentHeadUnderHead(Head headUnderHead)
    {
        _currentHeadUnderHead = headUnderHead;
    }
    
    private Head FindHeadUnderHead()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("head");
        if (Physics.Raycast(transform.position, -transform.up, out hit, 100, mask))
        {
            Debug.Log("ray cast");
            Head head = hit.collider.GetComponentInChildren<Head>();
            return head;
        }

        return null;
    }
    
    #endregion
}
