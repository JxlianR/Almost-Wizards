using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterBehavior : MonoBehaviour
{
    public GameObject[] Elements;
    public GameObject[] CombinedElements;
    public GameObject[] Enemies;

    public GameObject[] Players;

    public Transform[] EnemySpawnPoints;
    public Transform[] SpawnPoints;
    public Transform spawnPoint;

    private Animator animator;

    public static Vector3 playerPosition;
    private Vector3 enemySpawnPosition;

    public float maxZ = 27; // Maximal z-value the grandmaster can spawn at
    public float maxX = 26; // Maximal x-value the grandmaster can spawn at

    public static int randomPlayer;
    public int HP;
    public int timeBetweenSpell;
    public int timeBetweenArea;
    public int timeBetweenEnemy;

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
        HandleAnimation();

        transform.LookAt(playerPosition);

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void HandleAnimation()
    {

    }

    IEnumerator Grandmaster()
    {
        //float xPosition = Random.Range(maxX, -maxX);
        //float zPosition = Random.Range(maxZ, -maxZ);
        //transform.position = DetectGroundHeight(xPosition, zPosition) + new Vector3(0, 0.8f, 0);
        //randomPlayer = Random.Range(0, Players.Length);

        int randomSpawnPoint = Random.Range(0, SpawnPoints.Length);
        transform.position = SpawnPoints[randomSpawnPoint].position;

        for (int i = 0; i < 4; i++)
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
        float randomX = Random.Range(maxX, -maxX);
        float randomZ = Random.Range(maxZ, -maxZ);
        int randomCombinedElement = Random.Range(0, CombinedElements.Length);

        if (DetectGroundHeight(randomX, randomZ).y <= 1)
        {
            Instantiate(CombinedElements[randomCombinedElement], DetectGroundHeight(randomX, randomZ) + new Vector3(0, 0.5f, 0), transform.rotation);
            yield return new WaitForSeconds(timeBetweenArea);
        }

        StartCoroutine(CombinedAreas());
    }

    IEnumerator SpawnEnemies()
    {
        //int randomSpawnPoint = Random.Range(0, EnemySpawnPoints.Length);

        //Instantiate(Enemies[randomEnemy], EnemySpawnPoints[randomSpawnPoint].position, transform.rotation);
        yield return new WaitForSeconds(2);

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

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        return hit.point; // returning the position of the ground
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
