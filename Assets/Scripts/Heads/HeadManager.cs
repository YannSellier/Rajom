using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections.Generic;
public class HeadManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] 
    private string inputParam;
    
    [SerializeField]
    private PlayerInput playerInput;

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
        if (playerInput.actions[inputParam].WasPressedThisFrame())
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
