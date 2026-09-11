using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class ExplorationController : MonoBehaviour
{
    // This script controls the exploration based rooms
    private BoxCollider2D roomCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject player;
    [SerializeField] private enemySpawner spawnerScript;
    [SerializeField] private Transform spawnLocationParent;
    private List<GameObject> livingEnemies = new List<GameObject>();
    private Transform[] spawnLocations;

    void Awake()
    {
        roomCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");
        spawnLocations = spawnLocationParent.GetComponentsInChildren<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < livingEnemies.Count; i++)
            {
                Destroy(livingEnemies[i]);
            }
        }
    }

    void enemyDeath(GameObject enemy)
    {
        livingEnemies.Remove(enemy);
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            int type = Random.Range(0, System.Enum.GetValues(typeof(enemyController.EnemyTypes)).Length);
            GameObject enemy = spawnerScript.Spawn((enemyController.EnemyTypes)type, spawnLocations[i].position);
            enemy.GetComponent<enemyController>().death += enemyDeath;
            enemyController enemyControllerScript = enemy.GetComponent<enemyController>();
            enemyControllerScript.exploration = true;
            livingEnemies.Add(enemy);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
