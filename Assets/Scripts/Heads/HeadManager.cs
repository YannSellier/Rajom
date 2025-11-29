using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
using DefaultNamespace;

public class HeadManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField] private EInput changeHeadInput = EInput.ChangeHead1;

    private List<Head> heads;
    private int currentHeadIndex = 0;
    
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        RefreshHeadVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions[changeHeadInput.ToString()].WasPressedThisFrame())
            PickNextHead();
    }


    public void PickNextHead()
    {
        currentHeadIndex = (currentHeadIndex + 1) % heads.Count;
        RefreshHeadVisibility();
    }
    public Head GetCurrentHead()
    {
        return heads[currentHeadIndex];
    }
    public void RefreshHeadVisibility()
    {
        Head currentHead = GetCurrentHead();
        
        foreach (Head h in heads)
        {
            bool shouldBeActive = h == currentHead;
            h.gameObject.SetActive(shouldBeActive);
        }
    }
}
