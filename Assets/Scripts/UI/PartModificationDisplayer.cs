
using UnityEngine;
using UnityEngine.UIElements;

public class PartModificationDisplayer
{
    private Label _partModification_Label; 
    
    private PartModification _partModification;
    private VisualElement _root;
    
    public PartModificationDisplayer(PartModification partModification, VisualElement root)
    {
        _partModification = partModification;
        _root = root;
        _partModification_Label = root.Q<Label>("RecipePartModification_Label");
        
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        _partModification_Label.text = _partModification.GetHeadType().ToString(); 
    }
}
