using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AiNavDarkEnemy : MonoBehaviour
{
    public NavMeshAgent Agent;

    public int healthPoints;

    private GameObject[] Players; // Array with all Players

    private Transform closestPlayer;

    private bool gotDamage;

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
            Destroy(gameObject);
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
        if (other.tag != "Player" && gotDamage == false)
        {
            healthPoints -= ElementBehavior.damage; // Substracts the damage the element is doing from the HP
            Debug.Log("Healtpoints = " + healthPoints + " - " + ElementBehavior.damage);
            gotDamage = true; // true means the enemy got damage a short time ago
            StartCoroutine(CanGetDamage());
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

    IEnumerator CanGetDamage()
    {
        yield return new WaitForSeconds(0.5f);
        gotDamage = false; // Enemy can get damage again
    }
}
