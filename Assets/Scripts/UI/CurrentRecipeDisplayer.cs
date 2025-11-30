
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentRecipeDisplayer
{
    private Recipe _recipe;
    private VisualElement _root;
    private VisualTreeAsset _recipePart_assets;
    private VisualElement _currentPartsData_Container;
    
    private VisualElement[] _currentParts = new VisualElement[4];
    
    private VisualElement _currentPart1 = new VisualElement();
    private VisualElement _currentPart2 = new VisualElement();
    private VisualElement _currentPart3 = new VisualElement();
    private VisualElement _currentPart4 = new VisualElement();
    
    private List<EPartType> _partTypes = new List<EPartType> { EPartType.HANDLE, EPartType.ALIM, EPartType.HEAD, EPartType.PIPE };
    
    public CurrentRecipeDisplayer (Recipe recipe, VisualElement root, VisualTreeAsset recipePartAssets)
    {
        _recipe = recipe;
        _root = root;
        _recipePart_assets = recipePartAssets;

        // Recupere dans L'ui l'ensemble des zones
        // Une zone correspond a une part affiche
        _currentPart1 = root.Q<VisualElement>($"RecipeCurrentPart_1");
        _currentPart2 = root.Q<VisualElement>($"RecipeCurrentPart_2");
        _currentPart3 = root.Q<VisualElement>($"RecipeCurrentPart_3");
        _currentPart4 = root.Q<VisualElement>($"RecipeCurrentPart_4");

        for (int i = 0; i < 4; i++)
        {
            GetCurrentPartElement(i+1).Clear();
        }
        

        CreateCurrentRecipeDisplayer();
        DisplayCurrentParts();
    }
    
    public void RefreshUI()
    {
        DisplayCurrentParts(); 
    }

    // Pour chaque Zone instancie une carte
    public void CreateCurrentRecipeDisplayer()
    {
        for (int i = 0; i < 4; i++)
        {
            // Creer une instance d'une etape de la recette 
            VisualElement currentPart = _recipePart_assets.Instantiate();
            
            // ajoute cette instance a l'ui 
            GetCurrentPartElement(i+1).Add(currentPart);
        }
    }

  
    public void DisplayCurrentParts ()
    {
        // parcourt tout les zone contenant une partie de la recette
        for (int i = 0; i < 4; i++)
        {
            DisplayPartOfType(_partTypes[i], i);
        }
    }

    private Part GetPartOfType(EPartType partType)
    {
        List<Part> parts = GameManager.GetRef().GetParts();
        
        if ( parts == null || parts.Count <= 0)
        {
            return null; 
        }
        Part part = parts.Find((part => part.GetPartType() == partType));
        return part; 
    }
    
    public void DisplayPartOfType(EPartType partType, int index)
    {
        if (_recipe == null)
            return;
        
        // On recupere le slot qui corresponde a la partie de la reccete que l'on veut afficher
        Part part = GetPartOfType(partType);
        
        
        // on recuperere les modifications du slot 
        List<PartModification> partModifications = part?.GetModifications(); 
            
        // on recupere les modif pour le type part de la recette en  cours 
        List<PartModification>recipeModifications = _recipe.GetPartModificationsByTypeAndIndex(_partTypes[index]); 
            
            
        // comparaison des modification afin de trouver que est l'index de la recipe modifications du current slot
        int currentModificationIndex = FindCurrentModificationIndexInRecipe(recipeModifications, partModifications);
            
        // réécuper la prochaaine modif 
        int nextIndex = Mathf.Min(currentModificationIndex + 1, recipeModifications.Count - 1);
        PartModification nextModification = recipeModifications[nextIndex];
            
        

            
        // recuperer la zone dans lequel  on va modif noter element 
        GetCurrentPartElement(index+1).Q<VisualElement>("part").AddToClassList(GetCssForPartType(_partTypes[index])) ;
        GetCurrentPartElement(index+1).Q<VisualElement>("station").AddToClassList(GetCssForWorkStation(nextModification.GetWorkStationType())) ;
        GetCurrentPartElement(index+1).Q<VisualElement>("action").AddToClassList(GetCssForHeadType(nextModification.GetHeadType())) ;
    }

    
    
    private VisualElement GetCurrentPartElement(int index)
    {
        switch (index)
        {
            case 1: return _currentPart1;
            case 2: return _currentPart2;
            case 3: return _currentPart3;
            case 4: return _currentPart4;
            default: return null;
        }
    }
    

    public int FindCurrentModificationIndexInRecipe(
        List<PartModification> recipeModif, 
        List<PartModification> slotModif, 
        int index = 0)
    {
        // sécurité : éviter dépassement
        if (slotModif == null ||  index >= recipeModif.Count || index >= slotModif.Count)
            return 0;

        bool same =
            recipeModif[index].GetHeadType() == slotModif[index].GetHeadType() &&
            recipeModif[index].GetWorkStationType() == slotModif[index].GetWorkStationType();

        if (same)
        {
            return index; // correspondance trouvée
        }

        // appel récursif avec retour !
        return FindCurrentModificationIndexInRecipe(recipeModif, slotModif, index + 1);
    }
    
    public string GetCssForHeadType(EHeadType type)
    {
        return type switch
        {
            EHeadType.HAMMER => "hammer_action",
            EHeadType.SCREW => "drill_action",
            EHeadType.SAW => "saw_action",
            EHeadType.PLIERS => "welder_action",
            _ => string.Empty
        };
    }
    
    public string GetCssForPartType(EPartType type)
    {
        return type switch
        {
            EPartType.HANDLE => "handle_part",
            EPartType.ALIM => "alim_part",
            EPartType.HEAD => "head_part",
            EPartType.PIPE => "pipe_part",
            _ => string.Empty
        };
    }
    
    public string GetCssForWorkStation(EWorkStationType type)
    {
        return type switch
        {
            EWorkStationType.STAR => "purple_station",
            EWorkStationType.PLANET => "blue_station",
            EWorkStationType.ATOM => "green_station",
            EWorkStationType.CUBE => "pink_station",
            _ => string.Empty
        };
    }
}