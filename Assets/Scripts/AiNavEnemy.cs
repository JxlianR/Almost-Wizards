using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AiNavEnemy : MonoBehaviour
{
    public NavMeshAgent Agent;

    private GameObject[] Players; // Array with all Players

    private Transform closestPlayer;

    public int healthPoints;

    public float timeTillDeath = 0.5f;

    public string weakness;
    public string combinedWeaknessElement1; // Tag of the first element that can be created when combing the weakness with another element
    public string combinedWeaknessElement2; // Tag of the first element that can be created when combing the weakness with another element

    // Start is called before the first frame update
    void Start()
    {
        closestPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        closestPlayer = getClosestPlayer();
        Movement();
        Rotation();

        // Destroy the enemy when the HP is 0
        if (healthPoints <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == weakness || collision.collider.tag == combinedWeaknessElement1 || collision.collider.tag == combinedWeaknessElement2) // Checks if the tag of the object the enemy collides with is equals the weakness or a combined element of it
        {
            healthPoints -= ElementBehaviour.damage; // Substracts the damage the element is doing from the HP
            Debug.Log("Healtpoints = " + healthPoints + " - " + ElementBehaviour.damage);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == weakness || other.tag == combinedWeaknessElement1 || other.tag == combinedWeaknessElement2) // Checks if the tag of the object the enemy collides with is equals the weakness or a combined element of it
        {
            healthPoints -= ElementBehavior.damage; // Substracts the damage the element is doing from the HP
            Debug.Log("Healtpoints = " + healthPoints + " - " + ElementBehavior.damage);
        }
    }

    private void Movement()
    {
        //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime); // Moving the enemy forwards#
        Agent.SetDestination(closestPlayer.position);
    }

    private void Rotation()
    {
        // Rotating the enemy to the position of target GameObject
        transform.LookAt(closestPlayer);
        transform.Rotate(0, 90, 0);
    }

    public Transform getClosestPlayer()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity; // Setting a very high number to this variable
        Transform playerPosition = null;

        foreach (GameObject player in Players)
        {
            float currentDistance = Vector3.Distance(transform.position, player.transform.position); // Setting the distance of this gameobject and the player to the varibale currentDistance

            if (currentDistance < closestDistance) // Checking if the currentDistance is smaller than the closestDistance (which is a very high number)
            {
                closestDistance = currentDistance; // Setting the closestDistance to the currentDistance
                playerPosition = player.transform; // Seeting the Transform playerPosition to the position of the player
            }
        }
        return playerPosition;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
    }
}
