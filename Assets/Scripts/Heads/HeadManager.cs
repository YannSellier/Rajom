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
    [SerializeField] 
    private Head actualHead;
    void Start()
    {
        heads = new List<Head>(GetComponentsInChildren<Head>());
        setVoid();
        setHead(heads[3]);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions[inputParam].WasPressedThisFrame())
        {
            setHead(heads[1]);
        }
    }

    
    public void setHead(Head head)
    {
        foreach (Head h in heads)
        {
            h.gameObject.SetActive(h==head);
        }
    }

    public void setVoid()
    {
        foreach (Head h in heads)
        {
            h.gameObject.SetActive(false);
        }
    }

    public EHeadType getHeadTyp()
    {
        return actualHead.GetHeadType();
    }
}
