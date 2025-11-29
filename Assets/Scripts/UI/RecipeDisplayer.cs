
using UnityEngine.UIElements;

public class RecipeDisplayer
{
    private Label _nameLabel; 
    private Label _descriptionLabel;
    
    private Recipe _recipe; 
    private VisualElement _root; 
    
    public RecipeDisplayer(Recipe recipe, VisualElement root)
    {
        _recipe = recipe;
        _root = root;
            
        _nameLabel = root.Q<Label>("RecipeName_Label");
        _descriptionLabel = root.Q<Label>("RecipeDescription_Label");
        
        
    }

    public void SetRecipe(Recipe recipe)
    {
        _recipe = recipe;
        RefreshUI();
    }

    public void RefreshUI()
    {
        _nameLabel.text = _recipe.GetName();
        _descriptionLabel.text = _recipe.GetDescription();
    }
}
