using System.Collections.Generic;
using UnityEngine;

public class VacuumSpawner : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private Transform[] _spawnPoints = new Transform[4];

    [SerializeField] private GameObject[] _vacuumPrefabs = new GameObject[4];

    [SerializeField] private Transform _partsParent;

    [SerializeField] public List<Recipe> recipes;

    [SerializeField] public List<PartPrefab> partPrefabs;

    [SerializeField] private int recipeCount;
    
    #endregion

    #region GETTERS / SETTERS

    #endregion

    #region CONSTRUCTOR


    #endregion

    #region STATIC 

    private static VacuumSpawner _instance;
    public static VacuumSpawner GetRef()
    {
        if (_instance == null)
            _instance = FindObjectOfType<VacuumSpawner>();
        if (_instance == null)
            _instance = new GameObject("VacuumSpawner").AddComponent<VacuumSpawner>();
        return _instance;
    }

    #endregion

    
    public List<Recipe> CreateRecipes()
    {
        List<Recipe> recipes = new List<Recipe>();
        
        for (int i = 0; i < recipeCount; i++)
            recipes.Add(CreateRecipe(i));

        return recipes;
    }
    public Recipe CreateRecipe(int indexRecipe)
    {
        return new Recipe("oui", "oui", new List<PartData>()
        {
            CreatePartDataOfType(EPartType.HANDLE, indexRecipe),
            CreatePartDataOfType(EPartType.ALIM, indexRecipe ),
            CreatePartDataOfType(EPartType.PIPE, indexRecipe),
            CreatePartDataOfType(EPartType.HEAD, indexRecipe),
        });
    }
    public PartData CreatePartDataOfType(EPartType partType, int indexRecipe)
    {
        PartPrefab partPrefab = GetPartPrefabOfType(partType);

        int nbSteps = Mathf.Min(partPrefab.numberSteps, indexRecipe);
        if (nbSteps == 0)
            nbSteps = 1;
        List<PartModification> modifications = CreatePartModifications(nbSteps);
        PartData partData = new PartData(partType, modifications);
        partData.partPrefab = partPrefab.prefab;
        return partData;
    }
    
    public List<PartModification> CreatePartModifications(int maxSteps)
    {
        List<PartModification> modifications = new List<PartModification>();
        for (int i = 0; i < maxSteps; i++)
            modifications.Add(CreatePartModification());
        
        return modifications;
    }
    public PartModification CreatePartModification()
    {
        EHeadType[] possibleHeadTypes = new[]
        {
            EHeadType.HAMMER,
            EHeadType.PLIERS,
            EHeadType.SAW,
            EHeadType.SCREW,
        };

        EWorkStationType[] possibleWorkStationTypes = new[]
        {
            EWorkStationType.ATOM,
            EWorkStationType.CUBE,
            EWorkStationType.PLANET,
            EWorkStationType.STAR,
        };

        return new PartModification(
            possibleHeadTypes[Random.Range(0, 4)],
            possibleWorkStationTypes[Random.Range(0,4)]
            );
    }
    public PartPrefab GetPartPrefabOfType(EPartType partType)
    {
        List<PartPrefab> partPrefabsOfType = new List<PartPrefab>();
        foreach (PartPrefab partPrefab in partPrefabs)
            if (partPrefab.partType == partType)
                partPrefabsOfType.Add(partPrefab);

        int randomIndex = Random.Range(0, partPrefabsOfType.Count);
        return partPrefabsOfType[randomIndex];
    }




    //=============================================================================
    // SPAWNER
    //=============================================================================

    #region SPAWNING

    public void SpawnAllParts()
    {
        Recipe recipe = RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe();
        SpawnPartOfType(EPartType.HANDLE, recipe);
        SpawnPartOfType(EPartType.ALIM, recipe);
        SpawnPartOfType(EPartType.PIPE, recipe);
        SpawnPartOfType(EPartType.HEAD, recipe);
    }
    public void SpawnPartOfType(EPartType partType, Recipe recipe)
    {
        Part part = SpawnPartAtPoint(partType, recipe);
        BindPartEvents(part);
    }
    private Part SpawnPartAtPoint(EPartType partType, Recipe recipe)
    {
        Transform spawnPoint = GetSpawnPoint(partType);
        GameObject prefab = GetVacuumPrefab(partType, recipe);
        
        if (spawnPoint ==null)
            throw new System.NullReferenceException("Spawn point for part type " + partType + " is null.");
        if (prefab == null)
            throw new System.NullReferenceException("Prefab for part type " + partType + " is null.");
        
        GameObject partInstance = Instantiate(prefab, _partsParent);
        partInstance.transform.position = spawnPoint.position;
        partInstance.transform.localScale = Vector3.one;
        return partInstance.GetComponent<Part>();
    }

    #endregion

    #region PART EVENT 

    private void BindPartEvents(Part part)
    {
        part.onPartDeleted += HandlePartDeleted;
    }
    private void HandlePartDeleted(Part part)
    {
        part.onPartDeleted -= HandlePartDeleted;
        
        Recipe recipe = RecipesCreator.GetRef().GetRecipesesManager().GetCurrentRecipe();
        EPartType partType = part.GetPartType();
        SpawnPartOfType(partType, recipe);
    }
    #endregion
    
    
    
    #region UTILITIES
    private int GetPrefabIndex(EPartType partType)
    {
        switch (partType)
        {
            case EPartType.HANDLE:
                return 0;
            case EPartType.ALIM:
                return 1;
            case EPartType.PIPE:
                return 2;
            case EPartType.HEAD:
                return 3;
            default:
                return -1;
        }
    }
    private Transform GetSpawnPoint(EPartType partType)
    {
        int index = GetPrefabIndex(partType);
        if (index != -1)
            return _spawnPoints[index];
        return null;
    }
    private GameObject GetVacuumPrefab(EPartType partType, Recipe recipe)
    {
        return recipe.GetPartDataFromPartType(partType).partPrefab;
        int index = GetPrefabIndex(partType);
        if (index != -1)
            return recipe.partPrefabs[index];
        return null;
    }

    #endregion

        
}