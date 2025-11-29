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
    [SerializeField] private Transform _grabPosition;
    
    private List<Head> heads;
    
    
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        RefreshHeadVisibility();
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
    public void RefreshHeadVisibility()
    {
        foreach (Head h in heads)
        {
            bool shouldBeActive = _currentGrabbedHead != null && h.GetHeadType() == _currentGrabbedHead.GetHeadType();
            h.gameObject.SetActive(shouldBeActive);
        }
    }
    
    private void OnGrabInput()
    {
        if (_currentGrabbedHead != null)
        {
            Debug.Log("(bug)");
        }
        else
        {
            Debug.Log("try Grabbing");
            TryGrabbingHeadUnderHead();
        }
    }

    private void TryGrabbingHeadUnderHead()
    {
        if (_currentHeadUnderHead != null)
        {
            GrabHead(_currentHeadUnderHead);
            _currentHeadUnderHead = null;
        }
    }

    private void GrabHead(Head head)
    {
        if (head == null || _grabPosition == null)
            return;
        
        _currentGrabbedHead = _currentHeadUnderHead;
        RefreshHeadVisibility();
        head.gameObject.SetActive(false);
    }

    /*
    private void ReleasePart(Head head)
    {
        if (head == null)
            return;

        head.transform.
    }
    */
    
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
