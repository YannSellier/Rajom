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

    private static PartModification _drill = new PartModification(EHeadType.DRILL); 
    private static PartModification _saw = new PartModification(EHeadType.SAW); 
    private static PartModification _hammer = new PartModification(EHeadType.HAMMER); 
    private static PartModification _welder = new PartModification(EHeadType.WELDER); 
    
    private List<Recipe> _recipes = new List<Recipe>
    {
        new Recipe(
            "Recipe1",
            "Description de la première recette",
            new List<PartData>
            {
                new PartData(EPartType.ALIM, new List<PartModification>{_drill, _saw}),
                new PartData(EPartType.HANDLE, new List<PartModification>{_drill, _saw}),
                new PartData(EPartType.HEAD, new List<PartModification>{_hammer, _hammer}),
                new PartData(EPartType.PIPE, new List<PartModification>{_welder, _drill}),
            }
        ),
        new Recipe(
            "Recipe2",
            "Description de la seconde recette",
            new List<PartData>
            {
                new PartData(EPartType.ALIM, new List<PartModification>{_welder,_drill, _saw}),
                new PartData(EPartType.HANDLE, new List<PartModification>{_drill, _welder,_hammer}),
                new PartData(EPartType.HEAD, new List<PartModification>{_welder, _hammer,_saw}),
                new PartData(EPartType.PIPE, new List<PartModification>{_welder, _hammer,_drill }),
            }
        )
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
