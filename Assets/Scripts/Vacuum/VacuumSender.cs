using System.Collections.Generic;
using UnityEngine;

public class VacuumAssembler : MonoBehaviour
{
    //=============================================================================
    // VARIABLES
    //=============================================================================

    #region VARIABLES

    [SerializeField] private float _sendForce = 10f;
    [SerializeField] private float _sendDuration = 3f;
    
    private GameObject _currentVacuumObject;
    
    #endregion

    #region GETTERS / SETTERS


    #endregion

    #region STATIC

    private static VacuumAssembler _instance; 
    public static VacuumAssembler GetRef()
    {
        if (_instance == null)
            _instance = FindObjectOfType<VacuumAssembler>();
        if (_instance == null)
            _instance = new GameObject("VacuumAssembler").AddComponent<VacuumAssembler>();
        return _instance;
    }

    #endregion



    //=============================================================================
    // VACUUM SENDING
    //=============================================================================

    #region VACUUM SENDING

    public void SendVacuum()
    {
        Rigidbody vacuumRigidbody = _currentVacuumObject.GetComponent<Rigidbody>();
        if (vacuumRigidbody != null)
        {
            vacuumRigidbody.isKinematic = false;
            vacuumRigidbody.AddForce(Vector3.up * _sendForce, ForceMode.Impulse);
            Destroy(_currentVacuumObject, _sendDuration);
        }
    }
    

    #endregion

    #region VACUUM ASSEMBLING

    public void AssembleVacuum()
    {
        // get the parts from the VacuumSpawner
        RecipesManager recipesManager = RecipesCreator.GetRef().GetRecipesesManager();
        List<PartSlot> partSlots = recipesManager.GetPartsSlots();
        
        // Create an empty Vacuum object
        _currentVacuumObject = new GameObject("AssembledVacuum");
        _currentVacuumObject.AddComponent<Rigidbody>();
        _currentVacuumObject.transform.parent = transform;
        
        // Parent each part to the vacuum object
        foreach(PartSlot partSlot in partSlots)
        {
            Part part = partSlot.GetCurrentPart();

            Destroy(part.GetRigidbody());
            
            // Parent each part to the vacuum object
            part.transform.SetParent(_currentVacuumObject.transform);
            // Optionally, adjust the position and rotation of each part here
        }
    }

    #endregion

        
}