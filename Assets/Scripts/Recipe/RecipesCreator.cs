using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RecipesCreator
{
    //=============================================================================
    // VARIABLES
    //=============================================================================


    #region STATIC

    private static RecipesCreator _instance;
    public static RecipesCreator GetRef()
    {
        if (_instance == null)
            _instance = new RecipesCreator();
        
        return _instance;
    }

    #endregion

    #region VARIABLES

    private RecipesManager _recipesesManager;

    private static PartModification _drill = new PartModification(EHeadType.DRILL, EWorkStationType.ATOM); 
    private static PartModification _saw = new PartModification(EHeadType.SAW, EWorkStationType.CUBE); 
    private static PartModification _hammer = new PartModification(EHeadType.HAMMER, EWorkStationType.PLANET); 
    private static PartModification _welder = new PartModification(EHeadType.WELDER, EWorkStationType.STAR); 
    
    private List<Recipe> _recipes = new List<Recipe>
    {
        new Recipe(
            "Recipe1",
            "Description de la première recette",
            new List<PartData>
            {
                new PartData(EPartType.ALIM, new List<PartModification>{_drill, _drill, _drill, _drill}),
                new PartData(EPartType.HANDLE, new List<PartModification>{_saw}),
                new PartData(EPartType.HEAD, new List<PartModification>{_hammer, _hammer, _hammer}),
                new PartData(EPartType.PIPE, new List<PartModification>{_welder, _welder, _welder}),
            }
        ),
    };

    
    #endregion

    //=============================================================================
    // CONSTRUCTOR
    //=============================================================================

    public RecipesCreator()
    { 
        _recipesesManager = new RecipesManager(_recipes) ;
    }

    public RecipesManager GetRecipesesManager()
    {
        return _recipesesManager;
    }
}
