using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    // This script spawns enemies!
    
    public GameObject enemyPrefab;                                          // This is the prefab of the enemy
    public enemyStats charger;                                              // This is the stats SO for the charger
    public enemyStats bishop;                                              // This is the stats SO for the bishop

    [Header("Debugging")]
    [SerializeField] private bool spawnEnemyDebug;                          // This is the button that allows you to spawn an enemy
    [SerializeField] private Transform spawnLocDebug;                       // This is the location of the spawn
    [SerializeField] private enemyController.EnemyTypes typeDebug;          // This is how you select which enemy to spawn

    public GameObject Spawn(enemyController.EnemyTypes type, Vector2 spawnPosition) // This is how enemies actually spawn in. This is what you call to spawn an enemy.
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyScript = enemyInstance.GetComponent<enemyController>();
        enemyScript.enemyTypes = type;

        switch (type) // This is where it assigns stats. Add to this every time a new enemy is added
        {
            case enemyController.EnemyTypes.Charger:
                enemyScript.chosenStats = charger;
                StartCoroutine(DelayStart(enemyScript));
                break;
            case enemyController.EnemyTypes.Bishop:
                enemyScript.chosenStats = bishop;
                StartCoroutine(DelayStart(enemyScript));
                break;
            default:
                Debug.Log("Invalid enemy type!");
                break;
        }

        return enemyInstance;
    }

    void Update()
    {
        if (spawnEnemyDebug) // This just detects if you hit the button to spawn, and if so, spawns it.
        {
            Spawn(typeDebug, spawnLocDebug.transform.position);
            spawnEnemyDebug = !spawnEnemyDebug;
        }
    }

    private IEnumerator DelayStart(enemyController enemyScript) // This is the part I hate. It just needs a SLIGHT delay to load stuff in.
    {
        yield return new WaitForSeconds(0.05f);
        enemyScript.LateStart();
    }
}
