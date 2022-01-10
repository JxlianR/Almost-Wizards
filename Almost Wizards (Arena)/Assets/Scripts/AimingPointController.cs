using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class AimingPointController : MonoBehaviour
{
    public float movementSpeed;

    private Vector2 movementInput = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Movment of the aiming point
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement.Normalize();
        transform.Translate(movement * Time.deltaTime * movementSpeed, Space.World);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

}
