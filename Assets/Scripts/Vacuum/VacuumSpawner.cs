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
    
    #endregion

    #region GETTERS / SETTERS

    #endregion

    #region CONSTRUCTOR


    #endregion



    //=============================================================================
    // SPAWNER
    //=============================================================================

    #region SPAWNING

    public void SpawnAllParts()
    {
        SpawnPartOfType(EPartType.HANDLE);
        SpawnPartOfType(EPartType.ALIM);
        SpawnPartOfType(EPartType.PIPE);
        SpawnPartOfType(EPartType.HEAD);
    }
    public void SpawnPartOfType(EPartType partType)
    {
        Part part = SpawnPartAtPoint(partType);
        BindPartEvents(part);
    }
    private Part SpawnPartAtPoint(EPartType partType)
    {
        Transform spawnPoint = GetSpawnPoint(partType);
        GameObject prefab = GetVacuumPrefab(partType);
        
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
        
        EPartType partType = part.GetPartType();
        SpawnPartOfType(partType);
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
    private GameObject GetVacuumPrefab(EPartType partType)
    {
        int index = GetPrefabIndex(partType);
        if (index != -1)
            return _vacuumPrefabs[index];
        return null;
    }

    #endregion

        
}