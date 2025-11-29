
using UnityEngine;
using UnityEngine.UIElements;

public class PartDataDisplayer
{
    private Label _partType_Label; 
    
    private PartData _partData;
    private VisualElement _root;
    
    public PartDataDisplayer(PartData partData, VisualElement root)
    {
        _partData = partData;
        _root = root;
        _partType_Label = root.Q<Label>("RecipePartType_Label");
        
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        _partType_Label.text = _partData.GetPartType().ToString(); 
        
    }

    
}
