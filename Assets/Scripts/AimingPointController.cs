using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class AimingPointController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public CharacterController characterController;

    public float movementSpeed;

    private Vector2 movementInput = Vector2.zero;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 2.6f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Movment of the aiming point
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement.Normalize();
        characterController.Move(movement * Time.deltaTime * movementSpeed);

        // Adding gravity to our aiming point when it is not grounded
        moveVector = Vector3.zero;

        if (characterController.isGrounded == false) //Checks if character is grounded
        {
            moveVector += Physics.gravity; //Add gravity Vector
        }

        characterController.Move(moveVector * Time.deltaTime); //Apply move Vector
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

}
