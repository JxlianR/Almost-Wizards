using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterBehavior : MonoBehaviour
{
    private Animator animator;

    public GameObject[] Elements; // Array with all elements the grandmaster can shoot
    public GameObject[] CombinedElements; // Array with all combined elements that can spawn
    public GameObject[] Enemies; // Array with all enemies the grandmaster can spawn

    private GameObject[] Players; // Array with all players

    public Transform[] EnemySpawnPoints; // Array with spawnpoints for the enemies
    public Transform[] SpawnPoints; // Array with spawnpoints for the grandmaster
    public Transform spawnPoint; // Spawnpoint for the elements

    public ParticleSystem teleport;

    public static Vector3 playerPosition;

    private float maxZ = 27; // Maximal z-value objects can spawn at
    private float maxX = 26; // Maximal x-value objects can spawn at

    public static int randomPlayer;
    public int HP; // Healthpoints of grandmaster
    public int timeBetweenSpell; // Time between spawn of two spells
    public int timeBetweenArea; // Time between spawn of two areas
    public int timeBetweenEnemy; // Time between spawn of enemies

    private bool gotDamage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(Grandmaster());
        StartCoroutine(CombinedAreas());
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        playerPosition = Players[randomPlayer].transform.position;

        transform.LookAt(playerPosition);

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        return hit.point; // returning the position of the ground
    }

    IEnumerator Grandmaster()
    {
        int randomSpawnPoint = Random.Range(0, SpawnPoints.Length);
        Instantiate(teleport, transform.position, transform.rotation); // Spawns the particle effect for teleporting
        transform.position = SpawnPoints[randomSpawnPoint].position; // Teleports the grandmaster to a random spawnposition

        yield return new WaitForSeconds(2); // Waits for 2 seconds

        for (int i = 0; i < 4; i++) // Does the following loop 4 times
        {
            randomPlayer = Random.Range(0, Players.Length);

            int randomElement = Random.Range(0, Elements.Length);
            animator.SetBool("CastingSpell", true);
            Instantiate(Elements[randomElement], spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(EndAnimation());

            yield return new WaitForSeconds(timeBetweenSpell);
        }

        StartCoroutine(Grandmaster());
    }

    IEnumerator CombinedAreas()
    {
        float randomX = Random.Range(maxX, -maxX); // Random number between 26 & -26
        float randomZ = Random.Range(maxZ, -maxZ); // Random number between 27 & -27
        int randomCombinedElement = Random.Range(0, CombinedElements.Length); // Gets a random number between 0 and the length of the array with combined elements

        if (DetectGroundHeight(randomX, randomZ).y <= 1) // Checks if the y-value of the the position at randomX and randomZ is smaller than 1 -> Yes means there is no other object on the ground
        {
            Instantiate(CombinedElements[randomCombinedElement], DetectGroundHeight(randomX, randomZ) + new Vector3(0, 0.5f, 0), transform.rotation); // Spawns the combined elements
            yield return new WaitForSeconds(timeBetweenArea); // Waits a set time
        }

        StartCoroutine(CombinedAreas()); // Starts the coroutine again
    }

    IEnumerator SpawnEnemies()
    {
        //int randomSpawnPoint = Random.Range(0, EnemySpawnPoints.Length);

        //Instantiate(Enemies[randomEnemy], EnemySpawnPoints[randomSpawnPoint].position, transform.rotation);
        yield return new WaitForSeconds(4);

        for (int i = 0; i < EnemySpawnPoints.Length; i++)
        {
            int randomEnemy = Random.Range(0, Enemies.Length);
            Instantiate(Enemies[randomEnemy], EnemySpawnPoints[i].position, transform.rotation);
        }

        yield return new WaitForSeconds(timeBetweenEnemy);

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("CastingSpell", false);
    }

    IEnumerator CanGetDamage()
    {
        yield return new WaitForSeconds(4.3f);
        gotDamage = false; // Enemy can get damage again
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gotDamage == false && (other.tag != "Player" || other.tag != "Enemy"))
        {
            HP -= 1;
            gotDamage = true;
            StartCoroutine(CanGetDamage());
        }
    }
}
