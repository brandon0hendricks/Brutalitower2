using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveRoomController : MonoBehaviour
{
    // This script controls the wave rooms
    private int currentWave = 0;
    [SerializeField] private Transform spawnLocationParent;
    [SerializeField] private GameObject enterDoor;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private Transform enterTrigger;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject waveCounterObject;
    [SerializeField] private TextMeshProUGUI waveCounter;
    [SerializeField] private enemySpawner spawnerScript;
    [SerializeField] private int wave1EnemyCount;
    [SerializeField] private int wave2EnemyCount;
    [SerializeField] private int wave3EnemyCount;
    private List<GameObject> livingEnemies = new List<GameObject>();
    [SerializeField] private int waveTimer;
    private bool waveRunning = false;
    private bool betweenWaves = false;
    private Transform[] spawnLocations;
    private bool finished = false;


    void Awake()
    {
        spawnLocations = spawnLocationParent.GetComponentsInChildren<Transform>();
    }

    private bool enteredRoom()
    {
        if (!enterDoor.activeSelf)
            return Physics2D.OverlapCircle(enterTrigger.position, 0.2f, playerLayer);
        else
            return true;
    }

    void Update()
    {
        if (enteredRoom() && !finished)
        {
            enterDoor.SetActive(true);
            waveCounterObject.SetActive(true);
            waveCounter.text = currentWave.ToString();
            if (currentWave == 0)
            {
                currentWave++;
            }
        }
        if (currentWave == 1 && !waveRunning)
        {
            WaveSpawner(wave1EnemyCount);
        }
        else if (currentWave == 2 && !waveRunning)
        {
            WaveSpawner(wave2EnemyCount);
        }
        else if (currentWave == 3 && !waveRunning)
        {
            WaveSpawner(wave3EnemyCount);
        }

        if (livingEnemies.Count == 0 && enteredRoom() && !betweenWaves && !finished)
        {
            StartCoroutine(Timer(waveTimer));
        }
    }

    private void WaveSpawner(int enemyCount)
    {
        StartCoroutine(SpawnEnemies(enemyCount));
        waveRunning = true;
    }

    void enemyDeath(GameObject enemy)
    {
        livingEnemies.Remove(enemy);
    }

    private IEnumerator Timer(float time)
    {
        currentWave++;
        if (currentWave > 3)
        {
            waveCounterObject.SetActive(false);
            exitDoor.SetActive(true);
            finished = true;
            yield break;
        }
        betweenWaves = !betweenWaves;
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            waveCounter.text = timer.ToString("F1");
            yield return null;
        }
        waveRunning = false;
        betweenWaves = !betweenWaves;
    }

    private IEnumerator SpawnEnemies(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            int type = Random.Range(0, System.Enum.GetValues(typeof(enemyController.EnemyTypes)).Length);
            GameObject enemy = spawnerScript.Spawn((enemyController.EnemyTypes)type, spawnLocations[Random.Range(0, spawnLocations.Length)].position);
            enemy.GetComponent<enemyController>().death += enemyDeath;
            livingEnemies.Add(enemy);
            yield return new WaitForSeconds(0.25f);
        }

    }

}
