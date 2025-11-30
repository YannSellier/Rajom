using System.Collections;
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

    [SerializeField] private float _assemblingDuration = 1.5f;
    [SerializeField] private float _timeBeforeSending = 1.5f;
    
    private float _assemblingTimer = 0f;
    private Part[] _partsBeingAssembled;
    
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
        Rigidbody vacuumRigidbody = _currentVacuumObject.AddComponent<Rigidbody>();
        if (vacuumRigidbody != null)
        {
            vacuumRigidbody.isKinematic = false;
            vacuumRigidbody.useGravity = true;
            Vector3 explosionPosition = _currentVacuumObject.transform.position - Vector3.forward - Vector3.up;
            vacuumRigidbody.AddExplosionForce(_sendForce, explosionPosition, 5f, 1f, ForceMode.Impulse);
            Destroy(_currentVacuumObject, _sendDuration);
        }
        
        GameManager.GetRef().OnVacuumComplete();
    }
    

    #endregion

    #region VACUUM ASSEMBLING

    public void AssembleVacuum()
    {

        // get the parts from the VacuumSpawner
        RecipesManager recipesManager = RecipesCreator.GetRef().GetRecipesesManager();
        
        // Create an empty Vacuum object
        _currentVacuumObject = new GameObject("AssembledVacuum");
        _currentVacuumObject.transform.parent = transform;
        _currentVacuumObject.transform.localPosition = Vector3.zero;
        
        // Parent each part to the vacuum object
        _partsBeingAssembled = FindObjectsOfType<Part>();
        foreach(Part part in _partsBeingAssembled)
        {
            if (part == null)
                continue;
            
            // Delete the rigidbody if exists
            Rigidbody partRigidbody = part.GetComponent<Rigidbody>();
            if (partRigidbody != null)
                Destroy(partRigidbody);
            
            // Delete any collider if exists
            Collider[] partColliders = part.GetComponents<Collider>();
            foreach (Collider collider in partColliders)
            {
                Destroy(collider);
            }
            
            // Parent each part to the vacuum object
            part.transform.SetParent(_currentVacuumObject.transform);
        }
        
        // Start the assembling effect
        StartCoroutine(TriggerAssemblingEffect());
    }
    private IEnumerator TriggerAssemblingEffect()
    {
        _assemblingTimer = 0f;
        while (_assemblingTimer < _assemblingDuration)
        {
            UpdateAssemblingEffect();
            _assemblingTimer += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(_timeBeforeSending);
        
        SendVacuum();
    }
    private void UpdateAssemblingEffect()
    {
        for (int i = 0 ; i < _partsBeingAssembled.Length; i++)
        {
            Part part = GetPartByIndex(i);
            if (part == null)
                continue;
            
            UpdateAssemblingEffectOnPart(part);
        }
    }
    private void UpdateAssemblingEffectOnPart(Part part)
    {
        UpdateRotationAssemblingEffectOnPart(part);
        UpdatePositionAssemblingEffectOnPart(part);
    }
    
    #endregion

    #region ROTATION ASSEMBLING EFFECT

    private void UpdateRotationAssemblingEffectOnPart(Part part)
    {
        float remaningTime = Mathf.Max(_assemblingDuration - _assemblingTimer, 0.01f);
        Quaternion desiredRotation = Quaternion.identity;
            // Quaternion.Euler(part.GetAssemblingRotation());
        part.transform.rotation = Quaternion.Slerp(part.transform.rotation, desiredRotation, Time.deltaTime / remaningTime);
    }

    #endregion
    
    #region POSITION ASSEMBLING EFFECT
    
    private void UpdatePositionAssemblingEffectOnPart(Part part)
    {
        Vector3 desiredPosition = GetDesiredPartPosition(part);
        float remainingTime = Mathf.Max(_assemblingDuration - _assemblingTimer, 0.01f);
        part.transform.position = Vector3.Lerp(part.transform.position, desiredPosition, Time.deltaTime / remainingTime);
    }
    private Vector3 GetDesiredPartPosition(Part part)
    {
        Vector3 vacuumOrigin = _currentVacuumObject.transform.position;
        Vector3 vacuumDirection = -_currentVacuumObject.transform.forward;
        float partLengthOffset = GetPartLengthOffsetInVacuum(part);
        Vector3 desiredPosition = vacuumOrigin + vacuumDirection * partLengthOffset;
        return desiredPosition;
    }
    private float GetPartLengthOffsetInVacuum(Part part)
    {
        int index = GetPartIndexInVacuum(part);
        float offset = 0f;
        for (int i = 0; i < index; i++)
        {
            Part precedingPart = GetPartByIndexInVacuum(i);
            if (precedingPart != null)
                offset += precedingPart.GetAssemblingLength();
        }
        return offset;
    }
    private Part GetPartByIndexInVacuum(int index)
    {
        EPartType partType = GetPartsTypesOrder()[index];
        foreach (Part part in _partsBeingAssembled)
        {
            if (part.GetPartType() == partType)
                return part;
        }
        return null;
    }
    private int GetPartIndexInVacuum(Part part)
    {
        EPartType[] partTypesOrder = GetPartsTypesOrder();
        
        EPartType partType = part.GetPartType();
        for (int i = 0; i < partTypesOrder.Length; i++)
            if (partType == partTypesOrder[i])
                return i;
        
        return -1;
    }
    private Part GetPartByIndex(int index)
    {
        foreach (Part part in _partsBeingAssembled)
        {
            if (GetPartIndexInVacuum(part) == index)
                return part;
        }
        return null;
    }
    private EPartType[] GetPartsTypesOrder()
    {
        return new EPartType[]
        {
            EPartType.HANDLE,
            EPartType.ALIM,
            EPartType.PIPE,
            EPartType.HEAD,
        };
    }

    #endregion

        
}