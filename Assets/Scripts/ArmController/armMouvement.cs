using System;
using UnityEngine;
using UnityEngine.InputSystem;




public class armMouvement : MonoBehaviour
{
    /**
     *  Variable :
     */
    
    [SerializeField]
    private float movementSpeed = 2.0f;

    [SerializeField] 
    private Vector2 anchorPoint;
    [SerializeField]
    private float distanceFromAnchor = 5;
    private Vector2 move;
    private Vector2 velocity;

    [SerializeField]
    private PlayerInput playerInput;
    private Rigidbody rigidBody;

    [SerializeField] 
    private String inputParam;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector2(move.x * movementSpeed, move.y * movementSpeed);
        movement();
    }

    private void movement()
    {
        move = playerInput.actions[inputParam].ReadValue<Vector2>();
        Vector2 newVelocity = getNewVelocity();
        Vector2 arrivedPoint = new Vector2(transform.position.x, transform.position.z) + newVelocity * Time.deltaTime;
        clampVelocity(arrivedPoint);
    }

    private Vector2 getNewVelocity()
    {
        return new Vector2(rigidBody.position.x + velocity.x - anchorPoint.x, rigidBody.position.z + velocity.y - anchorPoint.y);
    }

    private void clampVelocity(Vector2 arrivedPoint)
    {
        
        if (!(Mathf.Sqrt(Mathf.Pow(arrivedPoint.x,2f) + Mathf.Pow(arrivedPoint.y, 2f)) > distanceFromAnchor))
        {
            setVelocity(new Vector3(velocity.x, 0.0f, velocity.y));
        }
        else
        {
            setVelocity(Vector3.zero);
        }
    }
    
    private void setVelocity(Vector3 velocity)
    {
        rigidBody.linearVelocity = velocity;
    }
}
