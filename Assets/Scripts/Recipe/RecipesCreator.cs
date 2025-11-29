using System.Collections.Generic;
using NUnit.Framework;

public class RecipesCreator
{
    //=============================================================================
    // VARIABLES
    //=============================================================================
    
    #region VARIABLES

    private RecipesManager _recipesesManager;  

    private List<Recipe> _recipes = new List<Recipe>
    {
        new Recipe(
            "Recipe1",
            "Description de la première recette",
            new List<TempPart>
            {
                new TempPart(EPartType.ALIM, new List<string> { "setup", "build" }),
                new TempPart(EPartType.HANDLE, new List<string> { "V8" }),
                new TempPart(EPartType.HEAD, new List<string> { "V8" }),
                new TempPart(EPartType.PIPE, new List<string> { "V8" }),
            }
        ),
        new Recipe(
            "Recipe2",
            "Description de la deuxième recette",
            new List<TempPart>
            {
                new TempPart(EPartType.ALIM, new List<string> { "setup", "build" }),
                new TempPart(EPartType.HANDLE, new List<string> { "V8" }),
                new TempPart(EPartType.HEAD, new List<string> { "V8" }),
                new TempPart(EPartType.PIPE, new List<string> { "V8" }),
            }
        ),
        new Recipe(
            "Recipe3",
            "Description de la troisième recette",
            new List<TempPart>
            {
                new TempPart(EPartType.ALIM, new List<string> { "setup", "build" }),
                new TempPart(EPartType.HANDLE, new List<string> { "V8" }),
                new TempPart(EPartType.HEAD, new List<string> { "V8" }),
                new TempPart(EPartType.PIPE, new List<string> { "V8" }),
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
