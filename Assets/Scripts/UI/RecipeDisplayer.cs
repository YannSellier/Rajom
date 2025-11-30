
using System.Collections.Generic;
using UnityEngine.UIElements;

public class RecipeDisplayer
{
    private Label _nameLabel; 
    private Label _descriptionLabel;
    
    private VisualElement _root;
    
    private VisualTreeAsset _recipePart_assets;
    private VisualElement _partData_Container;
    
    private VisualTreeAsset _recipePartModificationsAssets;
    private List<PartDataDisplayer> _partData_Displayers = new List<PartDataDisplayer>();
    
    public RecipeDisplayer(Recipe recipe, VisualElement root, VisualTreeAsset recipePartAssets, VisualTreeAsset recipePartModificationsAssets)
    {
        _root = root;
        _recipePart_assets = recipePartAssets;
        _recipePartModificationsAssets = recipePartModificationsAssets;
            
        _nameLabel = root.Q<Label>("RecipeName_Label");
        _descriptionLabel = root.Q<Label>("RecipeDescription_Label");
        _partData_Container = root.Q<VisualElement>("PartData_Container");
        
        CreatePartsDisplayer();
        RefreshUI();
    }

    public Recipe GetRecipe()
    {
        return RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe();
    }
    public void RefreshUI()
    {
        _nameLabel.text = GetRecipe().GetName();
        _descriptionLabel.text = GetRecipe().GetDescription();
    }
    
    public void CreatePartsDisplayer()
    {
        foreach (PartData partData in GetRecipe().GetParts())
        {
            CreatePartDisplayer(partData);
        }
    }

    public void CreatePartDisplayer(PartData partData)
    {
        VisualElement partDataDisplayer_root = _recipePart_assets.Instantiate();
        _partData_Container.Add(partDataDisplayer_root);
        PartDataDisplayer partDataDisplayer = new PartDataDisplayer(partData, partDataDisplayer_root, _recipePartModificationsAssets);
        _partData_Displayers.Add(partDataDisplayer);
    }
    
    
}
