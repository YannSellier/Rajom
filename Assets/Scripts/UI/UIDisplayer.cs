using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDisplayer : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    // properties
    private bool _isOpen = false;
    
    // ui refs
    private UIDocument _uiDocument;
    private VisualElement _root;

    #endregion

    #region GETTERS / SETTERS

    public bool IsOpen() => _isOpen;

    #endregion

    #region UI ELEMENT NAMES

    protected virtual string ROOT_NAME => throw new NotImplementedException();

    #endregion


    
    //=============================================================================
    // BUILT IN
    //=============================================================================

    #region BUILT IN

    protected void Awake()
    {
        Initialize();
    }
    
    #endregion

    

    //=============================================================================
    // UI DISPLAYER
    //=============================================================================

    #region SETUP


    protected virtual void Initialize()
    {
        FindUIReferences();
        BindListeners();
        
        RefreshUI();
    }
    protected virtual void FindUIReferences()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement.Q<VisualElement>(ROOT_NAME);
    }
    protected T FindVisualElement<T>(string name) where T : VisualElement
    {
        return _root.Q<T>(name);
    }
    protected virtual void BindListeners()
    {
        
    }
    protected virtual void UnbindListeners()
    {
        
    }
    protected virtual void Delete()
    {
        UnbindListeners();
    }

    #endregion

    #region REFRESH UI

    public virtual void RefreshUI() 
    {
        _root.style.display = _isOpen ? DisplayStyle.Flex : DisplayStyle.None;
    }

    #endregion

    #region OPENING CLOSING

    public void Open()
    {
        _isOpen = true;
        
        RefreshUI();
    }
    public void Close()
    {
        _isOpen = false;
        
        RefreshUI();
    }

    #endregion



}