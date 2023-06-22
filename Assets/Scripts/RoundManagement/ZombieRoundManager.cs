using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieRoundManager : MonoBehaviour
{
    public int startingRound = 1;
    public int zombiesPerRound = 10;
    public int zombiesPerRoundIncrease = 5;

    private int currentRound;
    [SerializeField] private int zombiesKilled;

    public GameObject zombiePrefab;
    public GameObject[] spawnPoints;

    public int zombiesToSpawn = 10;

    private int zombiesSpawned = 0;
    
    public Text roundText;


    private void Start()
    {
        currentRound = startingRound;
        zombiesKilled = 0;
        roundText.text = "Round: " + currentRound;

        StartRound();
    }

    private void Update()
    {
        // Check if the required number of zombies have been killed
        if (zombiesKilled >= zombiesPerRound)
        {
            EndRound();
            StartNextRound();
        }
    }

    private void StartRound()
    {
        Debug.Log("Starting Round " + currentRound);
        roundText.text = "Round: " + currentRound;
        int zombiesToSpawn = zombiesPerRound;
        zombiesSpawned = 0; // Reset the counter for spawned zombies
        StartCoroutine(SpawnZombies(zombiesToSpawn));
    }


    private void EndRound()
    {
        Debug.Log("Round " + currentRound + " completed!");
    }

    private void StartNextRound()
    {
        currentRound++;
        zombiesKilled = 0;
        zombiesPerRound += zombiesPerRoundIncrease;

        StartRound();
    }

    private IEnumerator SpawnZombies(int amount)
    {
        while (zombiesSpawned < amount)
        {
            // Get a random spawn point
            GameObject spawnPoint = GetRandomSpawnPoint();

            if (spawnPoint != null)
            {
                // Spawn a zombie at the spawn point
                Instantiate(zombiePrefab, spawnPoint.transform.position, Quaternion.identity);
                zombiesSpawned++;

                // Add a delay before spawning the next zombie
                yield return new WaitForSeconds(0.5f); // Adjust the delay duration as desired
            }
            else
            {
                // No more available spawn points
                break;
            }
        }
    }


    private GameObject GetRandomSpawnPoint()
    {
        // Shuffle the spawnPoints array to randomize the order
        for (int i = 0; i < spawnPoints.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, spawnPoints.Length);
            GameObject temp = spawnPoints[i];
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }

        // Find the first available and active spawn point
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint != null && spawnPoint.activeSelf)
            {
                return spawnPoint;
            }
        }

        return null; // No available spawn points
    }

    // Call this method when a zombie is killed
    public void ZombieKilled()
    {
        zombiesKilled++;
    }
}

