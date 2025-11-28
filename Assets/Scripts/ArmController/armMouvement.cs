using UnityEngine;
using UnityEngine.InputSystem;
public class armMouvement : MonoBehaviour
{
    private float mouvementSpeed = 2.0f;
    
    private Vector2 move;

    [SerializeField]
    private PlayerInput playerInput;
    
    private Rigidbody rigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move = playerInput.actions["Move"].ReadValue<Vector2>();
        rigidBody.linearVelocity = new Vector3(move.x * 20, 0.0f, move.y * 20);
    }
}
