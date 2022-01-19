using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    public CharacterController characterController;

    public Camera cam;

    public GameObject firstElement; // First projectile character can shoot
    public GameObject secondElement; // Second projectile character can shoot
    public GameObject oppositePlayer; // GameObject of the other player -> will spawn this gameObject if the other player is dead ans the next wave starts

    public float movementSpeed; // Speed of movement
    public float rotationSpeed; // Speed of rotation
    public float waitTime;

    public int lives; // Number of lives each player has

    public string firstElementTag; // Tag of the first element the player can shoot
    public string secondElementTag; // Tag of the second element the player can shoot

    private Vector2 movementInput;
    private Vector2 rotationInput;

    private Vector3 moveVector;

    public static Vector3 mousePositionLeft;
    public static Vector3 mousePositionRight;
    public static Vector3 aimPositionOne;
    public static Vector3 aimPositionTwo;

    private bool playerDead;
    private bool isMovingPressed;
    private bool playDieAnimation;

    public Transform spawnPoint; // Position of ElementSpawner (Gameobject) where the element will be spawned

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        isMovingPressed = movementInput.x != 0 || movementInput.y != 0; //isMovingPressed is true when the player is pressing WASD

        HandleAnimations();

        // Movement
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement.Normalize();
        characterController.Move(movement * Time.deltaTime * movementSpeed);

        // Adding gravity to our player when it is not grounded
        moveVector = Vector3.zero;

        if (characterController.isGrounded == false) //Checks if character is grounded
        {
            moveVector += Physics.gravity; //Add gravity Vector
        }

        characterController.Move(moveVector * Time.deltaTime); //Apply move Vector

        // Rotation towards mouse position
        Ray ray = cam.ScreenPointToRay(rotationInput);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 rotationPoint = ray.GetPoint(rayLength);
            Quaternion rotation = Quaternion.LookRotation(rotationPoint - transform.position);
            transform.rotation = rotation * Quaternion.Euler(0, 90, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime); // Character rotates from own rotation towards the position of the mouse with a set speed
            //transform.LookAt(new Vector3(rotationPoint.x, transform.position.y, rotationPoint.z));
        }

        // Checking if the player lost all lives and if so destroying it
        if (lives == 0)
        {
            //gameObject.SetActive(false);
            //playerDead = true;
            //playDieAnimation = true;
            //Destroy(gameObject);
            StartCoroutine(Dying());
        }

        if (LevelManager.playerCanSpawn == true)
        {
            Respawn();
        }

        /*if (LevelManager.spawnPlayer == true && playerDead)
        {
            gameObject.SetActive(true);
        }*/
    }

    private void HandleAnimations()
    {
        bool isMoving = animator.GetBool("IsMoving"); // Getting access to the bool

        if (isMovingPressed && !isMoving) // Checking if player presses WASD and the character is not moving
        {
            animator.SetBool("IsMoving", true); // Setting bool to true and trigger moving animation
        }
        else if (!isMovingPressed && isMoving) // Checking if player is not pressing WASD and the character is moving
        {
            animator.SetBool("IsMoving", false); // Setting bool to false and stop animation
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    public void MouseShootFirstElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameObject.FindGameObjectsWithTag(firstElementTag).Length == 0) // Checks if there is no other element of this type
            {
                GameObject.Instantiate(firstElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element

                // Getting the mouse position the momement the player left-clicks and assign it to a variable
                Ray ray = cam.ScreenPointToRay(rotationInput);
                Plane groundPlane = new Plane(Vector3.up, transform.position);
                float rayLength;

                if (groundPlane.Raycast(ray, out rayLength))
                {
                    mousePositionLeft = ray.GetPoint(rayLength);
                }
            }
        }
    }

    public void MouseShootSecondElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameObject.FindGameObjectsWithTag(secondElementTag).Length == 0) // Checks if there is no other element of this type
            {
                GameObject.Instantiate(secondElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element

                // Getting the mouse position the momement the player right-clicks and assign it to a variable
                Ray ray = cam.ScreenPointToRay(rotationInput);
                Plane groundPlane = new Plane(Vector3.up, transform.position);
                float rayLength;

                if (groundPlane.Raycast(ray, out rayLength))
                {
                    mousePositionRight = ray.GetPoint(rayLength);
                }
            }
        }
    }

    public void ControllerShootFirstElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameObject.FindGameObjectsWithTag(firstElementTag).Length == 0) // Checks if there is no other element of this type
            {
                GameObject.Instantiate(firstElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element

                aimPositionOne = GameObject.Find("AimingPoint").transform.position; // Setting the position of the aiming point to the variable aimPositionOne
            }
        }
    }

    public void ControllerShootSecondElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameObject.FindGameObjectsWithTag(secondElementTag).Length == 0) // Checks if there is no other element of this type
            {
                GameObject.Instantiate(secondElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element

                aimPositionTwo = GameObject.Find("AimingPoint").transform.position; // Setting the position of the aiming point to the variable aimPositionTwo
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") // Checks if the gameObject the player is colliding with is an enemy
        {
            lives -= 1; // Subtracting 1 life
            Destroy(other.gameObject); // Destroys the enemy
        }
        else if (other.tag == "Heart")
        {
            if (lives < 3)
            {
                lives++;
            }

            Destroy(other.gameObject);
        }
    }

    private void Respawn()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == 1)
        {
            Debug.Log("Reactivating the Player");
            Instantiate(oppositePlayer, new Vector3(0, 5, 0), Quaternion.identity);
            oppositePlayer.GetComponent<PlayerController>().lives = 1;
            LevelManager.playerCanSpawn = false;
        }
        else
        {
            LevelManager.playerCanSpawn = false;
        }
    }

    IEnumerator Dying()
    {
        animator.SetBool("PlayerDies", true);
        gameObject.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
