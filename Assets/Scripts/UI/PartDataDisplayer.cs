
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PartDataDisplayer
{
    private Label _partType_Label; 
    
    private PartData _partData;
    private VisualElement _root;
    
    private VisualTreeAsset _recipePartModificationsAssets;
    private List<PartModificationDisplayer> _partModifications_Displayers = new List<PartModificationDisplayer>();

    
    public PartDataDisplayer(PartData partData, VisualElement root, VisualTreeAsset recipePartModificationsAssets)
    {
        _partData = partData;
        _root = root;
        _partType_Label = root.Q<Label>("RecipePartType_Label");
        _recipePartModificationsAssets = recipePartModificationsAssets;
        
        CreatePartModificationsDisplayer();
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        _partType_Label.text = _partData.GetPartType().ToString(); 
    }
    
    public void CreatePartModificationsDisplayer()
    {
        foreach (PartModification partModification in _partData.GetModifications())
        {
            CreatePartModificationDisplayer(partModification);
        }
    }
    
    public void CreatePartModificationDisplayer(PartModification partModification)
    {
        VisualElement partModificationsDisplayer_root = _recipePartModificationsAssets.Instantiate();
        _root.Add(partModificationsDisplayer_root);
        PartModificationDisplayer partModificationDisplayer = new PartModificationDisplayer(partModification, partModificationsDisplayer_root);
        _partModifications_Displayers.Add(partModificationDisplayer);
    }
    
}
