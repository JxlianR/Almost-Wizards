using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject[] Players; // Array with all players
    private GameObject[] Enemies; // Array with all enemies

    public GameObject Player1;
    public GameObject Player2;

    private EnemySpawner enemySpawner;

    public int enemyMultiplicator; // Multiplicator for the number of enemies in the next wave

    private int waveNumber = 1;

    public static bool spawnPlayer;

    // Start is called before the first frame update
    void OnEnable()
    {
        Players = GameObject.FindGameObjectsWithTag("Player"); // Find all players and put them in the array

        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies and put them in the array

        if (AllEnemiesDead() && enemySpawner.enemyCount == enemySpawner.enemyAmount) // Checks if all enemies are dead and the number of enemies that should spawn did spawn
        {
            if (waveNumber < 10)
            {
                spawnPlayer = true;

                /*GameObject.Find("Player 1 (Keyboard)").SetActive(true); // Spawns Player1
                GameObject.Find("Player 1 (Keyboard)").GetComponent<PlayerController>().lives = 1;

                GameObject.Find("Player 2 (Controller)").SetActive(true); // Spawns Player2
                GameObject.Find("Player 2 (Controller)").GetComponent<PlayerController>().lives = 1;*/

                enemySpawner.enemyCount = 0;
                enemySpawner.enemyAmount *= enemyMultiplicator; // multiplies the amount of enemies that should spawn with an int
                StartCoroutine(enemySpawner.SpawnEnemies()); // Starts the coroutine to spawn new enemies

                waveNumber++;
                Debug.Log("Wave " + waveNumber + " starts now!");
            }
        }

        // Loads the scene again if all players are dead
        if (AllPlayersDead())
        {
            GameOver();
        }

        if (waveNumber == 4)
        {
            Win();
        }
    }

    // Checks if there are any players in the scene
    bool AllPlayersDead()
    {
        foreach (GameObject player in Players)
        {
            if (player != null)
            {
                return false;
            }
        }

        return true;
    }

    // Checks if there are any enemies in the scene
    bool AllEnemiesDead()
    {
        foreach (GameObject enemy in Enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }

        return true;
    }

    void Win()
    {
        Debug.Log("You have won the game! Congratulations, you are now a wizard!");
    }

    void GameOver()
    {
        SceneManager.LoadScene(0);
        Debug.Log("All player died, try again.");
    }
}
