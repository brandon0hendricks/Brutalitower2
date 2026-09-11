using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Permissions;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    private bool inRange;
    [SerializeField]
    private Player_Stats playerStats;
    [SerializeField] Upgrade upgradePrefab;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }

    void Start()
    {
        inRange = false; // Initialize inRange to false
        //Mostly for testing purposes
        upgradePrefab.IsPurchasable = true; // Ensure the upgrade is purchasable at the start
        if (upgradePrefab == null)
        {
            Debug.LogError("Upgrade Prefab is not assigned in the inspector.");
        }
        if (playerStats == null)
        {
            Debug.LogError("Player Stats is not assigned in the inspector.");
        }
    }
    void Update()
    {
        if (inRange)
        {
            //e to purchase upgrade
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("Pressing the E key.");
                //check if upgrade is purchasable or not (i.e. has been purchased before)
                if (upgradePrefab.IsPurchasable)
                {
                    switch(upgradePrefab.PurchaseType)
                    {
                        case "bonedust":
                            Debug.Log("Upgrade Type: Bone Dust");
                            //if the player has enough bone dust, purchase and spawn upgrade
                            if (playerStats.Bonedust >= upgradePrefab.UpgradePrice)
                            {
                                Debug.Log("Upgrade Purchased.");
                                upgradePrefab.IsPurchasable = false;
                                playerStats.Bonedust -= upgradePrefab.UpgradePrice;
                                Instantiate(upgradePrefab, new Vector3(transform.position.x - 4, transform.position.y, transform.position.z), Quaternion.identity);
                            }
                            break;
                        case "goldcoin":
                            Debug.Log("Upgrade Type: Gold Coin");
                            //if the player has enough gold coins, purchase and spawn upgrade
                            if (playerStats.Goldcoins >= upgradePrefab.UpgradePrice)
                            {
                                Debug.Log("Upgrade Purchased.");
                                upgradePrefab.IsPurchasable = false;
                                playerStats.Goldcoins -= upgradePrefab.UpgradePrice;
                                Instantiate(upgradePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                            }
                            break;
                        default:
                            Debug.LogWarning("Unknown Purchase Type: " + upgradePrefab.PurchaseType);
                            break;
                    }
                    /*
                    //if the player has enough money, purchase and spawn upgrade
                    if (playerStats.Money >= upgradePrefab.UpgradePrice)
                    {
                        Debug.Log("Upgrade Purchased.");
                        upgradePrefab.IsPurchasable = false;
                        playerStats.Money -= upgradePrefab.UpgradePrice;
                        Instantiate(upgradePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    */
                }

            }
        }
    }

}
