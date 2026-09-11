using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> upgradeList;
    public List<GameObject> availableUpgrades;
    public List<Sprite> icon_list;

    [SerializeField]
    GameObject upgradePrefab; // Prefab for the upgrade object

    private int numUniqueUpgrades;

    //removing the stock system, only one upgrade of each type can be available at a time
    private List<int> upgradeStock; // Array to hold the stock of each upgrade

    /*
    private void fillUpgradeList()
    {
        //Order: bool isPurchasable, string currency, int price; string type, float amount, bool isBase
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "hp", 10f, false, icon_list[0]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "damage", 5f, false, icon_list[1]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "jump", 1f, false, icon_list[2]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "dodge", 1f, false, icon_list[3]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "hp", 10f, true, icon_list[0]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "damage", 5f, true, icon_list[1]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "jump", 1f, true, icon_list[2]));
        upgradeList.Add(new Upgrade(true, "bonedust", 10, "dodge", 1f, true, icon_list[3]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "hp", 10f, false, icon_list[0]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "damage", 5f, false, icon_list[1]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "jump", 1f, false, icon_list[2]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "dodge", 1f, false, icon_list[3]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "hp", 10f, true, icon_list[0]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "damage", 5f, true, icon_list[1]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "jump", 1f, true, icon_list[2]));
        upgradeList.Add(new Upgrade(true, "goldcoin", 10, "dodge", 1f, true, icon_list[3]));

    }

    private void randomizeUpgrades(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int randomIndex = Random.Range(0, upgradeList.Count);
            Upgrade selectedUpgrade = upgradeList[randomIndex];
            // Check if the upgrade is already in the availableUpgrades list
            if (!availableUpgrades.Contains(selectedUpgrade))
            {
                availableUpgrades.Add(selectedUpgrade);
                upgradeStock.Add(1); // Initialize stock for this upgrade
            }
            else
            {
                //not using stock anymore
                // If already exists, increment the stock
                //int index = availableUpgrades.IndexOf(selectedUpgrade);
                //upgradeStock[index]++;
                while (availableUpgrades.Contains(selectedUpgrade))
                {
                    randomIndex = Random.Range(0, upgradeList.Count);
                    selectedUpgrade = upgradeList[randomIndex];
                }
                availableUpgrades.Add(selectedUpgrade);
            }
        }
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        //fillUpgradeList();
        //randomizeUpgrades(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
