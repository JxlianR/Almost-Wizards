using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterBehavior : MonoBehaviour
{
    public GameObject[] Elements;
    public GameObject[] CombinedElements;
    public GameObject[] Enemies;

    private  GameObject[] Players;

    public Transform[] EnemySpawnPoints;
    public Transform spawnPoint;

    public static Vector3 playerPosition;

    public float maxZ = 27; // Maximal z-value the grandmaster can spawn at
    public float maxX = 26; // Maximal x-value the grandmaster can spawn at

    public static int randomPlayer;
    public int HP;

    public bool gotDamage;

    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");

        StartCoroutine(Grandmaster());
        StartCoroutine(CombinedAreas());
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = Players[randomPlayer].transform.position;

        transform.LookAt(playerPosition);

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Grandmaster()
    {
        float xPosition = Random.Range(maxX, -maxX);
        float zPosition = Random.Range(maxZ, -maxZ);
        transform.position = DetectGroundHeight(xPosition, zPosition) + new Vector3(0, 0.8f, 0);
        //randomPlayer = Random.Range(0, Players.Length);

        for (int i = 0; i < 4; i++)
        {
            randomPlayer = Random.Range(0, Players.Length);

            int randomElement = Random.Range(0, Elements.Length);
            Instantiate(Elements[randomElement], spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(3f);
        }

        StartCoroutine(Grandmaster());
    }

    IEnumerator CombinedAreas()
    {
        float randomX = Random.Range(maxX, -maxX);
        float randomZ = Random.Range(maxZ, -maxZ);
        int randomCombinedElement = Random.Range(0, CombinedElements.Length);

        Instantiate(CombinedElements[randomCombinedElement], DetectGroundHeight(randomX, randomZ), transform.rotation);
        yield return new WaitForSeconds(4);

        StartCoroutine(CombinedAreas());
    }

    IEnumerator SpawnEnemies()
    {
        int randomEnemy = Random.Range(0, Enemies.Length);
        int randomSpawnPoint = Random.Range(0, EnemySpawnPoints.Length);

        Instantiate(Enemies[randomEnemy], EnemySpawnPoints[randomSpawnPoint].position, transform.rotation);

        yield return new WaitForSeconds(6);

        StartCoroutine(SpawnEnemies());
    }

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        //Debug.Log("Terrain location found at" + hit.point);
        return hit.point; // returning the position of the ground
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gotDamage == false)
        {
            HP -= 1;
            gotDamage = true;
            StartCoroutine(CanGetDamage());
        }
    }

    IEnumerator CanGetDamage()
    {
        yield return new WaitForSeconds(4.3f);
        gotDamage = false; // Enemy can get damage again
    }
}
