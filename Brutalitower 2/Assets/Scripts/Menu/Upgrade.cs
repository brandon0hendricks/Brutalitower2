using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{

    private List<string> upgradeDescriptions = new List<string>();

    private bool inRange;
    [SerializeField]
    private Player_Stats baseStats;
    [SerializeField]
    private Player_Stats playerStats;
    //[SerializeField]
    // private UpgradeScriptableObject upgrade;
    //[SerializeField]
    //private HealthbarUI healthbar;

    [SerializeField]
    private Sprite upgradeIcon; //what is the icon

    [SerializeField]
    private Sprite bonedustIcon; //icon for bonedust currency
    [SerializeField]
    private Sprite goldcoinIcon; //icon for gold coin currency
    private GameObject upgradeText; //text that shows the upgrade information
    private GameObject currencyText; //text that displays the upgrade price
    private GameObject currencyIcon; //icon that displays the currency type (bonedust or gold coin)

    [SerializeField]
    private bool isBaseUpgrade;
    [SerializeField]
    //If true, the item is part of the shop. If false, the item does not need to be purchased to be picked up
    //Feel like I need to clarify since the name can be confusing
    private bool isPurchasable;
    [SerializeField]
    private int upgradePrice;
    [SerializeField]
    private string upgradeType;
    [SerializeField]
    private float upgradeAmount;
    [SerializeField]
    private string purchaseType; // This can be used to differentiate between different types of purchases if needed

    // This is used to scale the text and icon size
    private float scaleFactor = 0.6f;
    //Factor for the position, not sure if this is needed or not yet
    private float positionFactor = 0.5f;

    private int upgradeFontSize = 8; // Font size for the upgrade text
    private int currencyFontSize = 2; // Font size for the currency text

    public void Start()
    {
        makeDescriptions(); // Initialize upgrade descriptions
        configureUpgradeUI(); // Configure the UI for the upgrade text and currency text
        inRange = false; // Initialize inRange to false
        upgradeText.SetActive(false); // Initially hide the upgrade text
        currencyText.SetActive(false); // Initially hide the currency text
        currencyIcon.SetActive(false); // Initially hide the currency icon
    }
    void Update()
    {
        //When the player is in range, check if the player can purchase the upgrade and show the upgrade text and currency text
        if (inRange)
        {
            UnityEngine.Debug.Log("Player is in range of the upgrade.");
            switch (purchaseType)
            {
                case "bonedust":
                    if (playerStats.Bonedust >= upgradePrice)
                    {
                        currencyText.GetComponent<TMPro.TextMeshPro>().color = Color.green; // If player has enough bonedust, set text color to green
                    }
                    else
                    {
                        currencyText.GetComponent<TMPro.TextMeshPro>().color = Color.red; // If not enough bonedust, set text color to red
                    }
                    break;
                case "goldcoin":
                    if (playerStats.Goldcoins >= upgradePrice)
                    {
                        currencyText.GetComponent<TMPro.TextMeshPro>().color = Color.green; // If player has enough gold coins, set text color to green
                    }
                    else
                    {
                        currencyText.GetComponent<TMPro.TextMeshPro>().color = Color.red; // If not enough gold coins, set text color to red
                    }
                    break;
            }
            if (IsPurchasable)
            {
                currencyText.SetActive(true); // Show the currency text
                currencyIcon.SetActive(true); // Show the currency icon
            }
            upgradeText.SetActive(true); // Show the upgrade text

            //if player is in range and presses the E key, check if the upgrade is purchasable
            if (Input.GetKeyDown("e"))
            {
                if (isPurchasable)
                {
                    switch (purchaseType)
                    {
                        case "bonedust":
                            if (playerStats.Bonedust >= upgradePrice)
                            {
                                playerStats.Bonedust -= upgradePrice; // Deduct the price from player's bonedust
                                ApplyUpgrade(); // Apply the upgrade to the player stats
                            }
                            else
                            {
                                UnityEngine.Debug.Log("Not enough Bone Dust to purchase upgrade.");
                                return;
                            }
                            break;
                        case "goldcoin":
                            if (playerStats.Goldcoins >= upgradePrice)
                            {
                                playerStats.Goldcoins -= upgradePrice; // Deduct the price from player's gold coins
                                ApplyUpgrade(); // Apply the upgrade to the player stats
                            }
                            else
                            {
                                UnityEngine.Debug.Log("Not enough Gold Coins to purchase upgrade.");
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    ApplyUpgrade(); // If the upgrade is not purchasable, apply the upgrade directly
                }
            }
        }
        //Set upgrade text and currency text to invisible when out of range
        else
        {
            UnityEngine.Debug.Log("Player is out of range of the upgrade.");
            currencyIcon.SetActive(false);
            currencyText.SetActive(false);
            upgradeText.SetActive(false);
        }
    }
    public string PurchaseType
    {
        get { return purchaseType; }
        set { purchaseType = value; }
    }
    public bool IsPurchasable
    {
        get { return isPurchasable; }
        set { isPurchasable = value; }
    }
    public float UpgradeAmount { get { return upgradeAmount; } set { upgradeAmount = value; } }
    public int UpgradePrice
    {
        get { return upgradePrice; }
        set
        {
            if (value >= 0) //price cannot be negative
            {
                upgradePrice = value;
            }
            else
            {
                upgradePrice = 0;
            }
        }
    }
    public string UpgradeType
    {
        get { return upgradeType; }
        set { upgradeType = value; }
    }
    public bool IsBaseUpgrade
    {
        get { return isBaseUpgrade; }
        set { isBaseUpgrade = value; }
    }

    public Upgrade()
    {
        isPurchasable = true; // Default value, can be set in the inspector
        purchaseType = "bonedust"; // Default value, can be set in the inspector
        upgradePrice = 10; // Default value, can be set in the inspector
        upgradeType = "hp"; // Default value, can be set in the inspector
        upgradeAmount = 10f; // Default value, can be set in the inspector
        isBaseUpgrade = false; // Default value, can be set in the inspector
    }

    public Upgrade(bool isPurchasable, string currency, int price, string type, float amount, bool isBase, Sprite icon)
    {
        this.isPurchasable = isPurchasable;
        this.upgradePrice = price;
        this.upgradeType = type;
        this.upgradeAmount = amount;
        this.purchaseType = currency;
        this.isBaseUpgrade = isBase;
        this.isBaseUpgrade = isBase;
        this.upgradeIcon = icon;
    }

    public void updatePrefab(bool isPurchasable, string currency, int price, string type, float amount, bool isBase, Sprite icon)
    {
        this.isPurchasable = isPurchasable;
        this.upgradePrice = price;
        this.upgradeType = type;
        this.upgradeAmount = amount;
        this.purchaseType = currency;
        this.isBaseUpgrade = isBase;
        this.upgradeIcon = icon;
    }

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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            upgradeText.SetActive(false); // Hide the upgrade text when player exits the trigger
            currencyText.SetActive(false); // Hide the currency text when player exits the trigger
            currencyIcon.SetActive(false); // Hide the currency icon when player exits the trigger
        }
    }

    private void ApplyUpgrade()
    {
        if (isBaseUpgrade)
        {
            switch (upgradeType)
            {
                case "hp":
                    baseStats.Max_Health += upgradeAmount;
                    baseStats.Current_Health = baseStats.Max_Health;
                    break;
                case "jump":
                    baseStats.JumpPower += upgradeAmount; break;
                case "dodge":
                    baseStats.Dodges += 1;
                    break;
                case "damage":
                    baseStats.Damage += upgradeAmount; break;
            }
        }
        else
        {
            switch (upgradeType)
            {
                case "hp":
                    playerStats.Max_Health += upgradeAmount;
                    playerStats.Current_Health = playerStats.Max_Health;
                    break;
                case "jump":
                    playerStats.JumpPower += upgradeAmount; break;
                case "dodge":
                    playerStats.Dodges += 1;
                    break;
                case "damage":
                    playerStats.Damage += upgradeAmount; break;
            }
        }
        Destroy(gameObject);
        UnityEngine.Debug.Log("Upgrade Acquired.");
    }
    private void configureUpgradeUI()
    {
        //create text objects and set the parent to this GameObject
        upgradeText = new GameObject("UpgradeText");
        currencyText = new GameObject("CurrencyText");
        currencyIcon = new GameObject("CurrencyIcon");

        upgradeText.layer = LayerMask.NameToLayer("UI"); // Set layer for upgrade text
        currencyText.layer = LayerMask.NameToLayer("UI"); // Set layer for currency text
        currencyIcon.layer = LayerMask.NameToLayer("UI"); // Set layer for currency icon

        upgradeText.transform.SetParent(gameObject.transform); // Set the parent of upgradeText to this GameObject
        currencyText.transform.SetParent(gameObject.transform); // Set the parent of currencyText to this GameObject
        currencyIcon.transform.SetParent(currencyText.transform); // Set the parent of currencyIcon to CurrencyText

        //add and change all necessary components to the text objects, and set the icon for the upgrade
        upgradeText.AddComponent<TMPro.TextMeshPro>(); // Add TextMeshPro component for upgrade text
        currencyText.AddComponent<TMPro.TextMeshPro>(); // Add TextMeshPro component for currency text
        currencyIcon.AddComponent<SpriteRenderer>(); // Add the image for the currency type
        gameObject.GetComponent<SpriteRenderer>().sprite = upgradeIcon; //Gets what shows icon

        //I believe there was an error where the getUpgradeText() function was returning a reference to the upgrade rather than the string itself, so I just returned the index and did it this way
        TMPro.TextMeshPro upgradeTextComponent = upgradeText.GetComponent<TMPro.TextMeshPro>();
        upgradeTextComponent.text = upgradeDescriptions[getUpgradeTextIndex()]; //uses the getUpgradeText() method to set the text
        TMPro.TextMeshPro currencyTextComponent = currencyText.GetComponent<TMPro.TextMeshPro>();
        currencyTextComponent.text = upgradePrice.ToString(); //sets the price text
        SpriteRenderer currencyIconComponent = currencyIcon.GetComponent<SpriteRenderer>();
        currencyIcon.GetComponent<SpriteRenderer>().sprite = (purchaseType == "bonedust") ? bonedustIcon : goldcoinIcon; //sets the currency icon based on purchase type
        
        // Set the position of the text and icon
        RectTransform upgradeTextRect = upgradeText.GetComponent<RectTransform>();
        upgradeTextRect.localPosition = new Vector3(0, 3f * positionFactor, 0); // Position above the upgrade object
        RectTransform currencyTextRect = currencyText.GetComponent<RectTransform>();
        currencyTextRect.localPosition = new Vector3(0, -1.5f * positionFactor, 0); // Position below the upgrade object
        Transform currencyIconTransform = currencyIcon.GetComponent<Transform>();
        currencyIconTransform.localPosition = new Vector3(-1f * positionFactor, 0, 0); // Position to the left of the currency text
        
        //Set the sizes of the text and icon
        upgradeTextRect.sizeDelta = new Vector2(5f * scaleFactor, 2f * scaleFactor); // Set size for upgrade text
        currencyTextRect.sizeDelta = new Vector2(1f * scaleFactor, 1f * scaleFactor); // Set size for currency text
        currencyIconTransform.localScale = new Vector2(2f * scaleFactor, 2f * scaleFactor); // Set size for currency icon
        
        //set font size
        upgradeTextComponent.enableAutoSizing = true; // Enable auto-sizing for the upgrade text
        upgradeTextComponent.fontSizeMin = upgradeFontSize / 2; // Set minimum font size for upgrade text
        upgradeTextComponent.fontSizeMax = upgradeFontSize * 4; // Set maximum font size for upgrade text
        currencyTextComponent.enableAutoSizing = true; // Enable auto-sizing for the currency text
        currencyTextComponent.fontSizeMin = currencyFontSize / 2; // Set minimum font size for currency text
        currencyTextComponent.fontSizeMax = currencyFontSize * 4; // Set maximum font size for currency text
        //upgradeTextComponent.fontSize = upgradeFontSize;
        //currencyTextComponent.fontSize = currencyFontSize;

        //Make sure that the text and icons are in the foreground and not hidden behind the player
        upgradeTextComponent.sortingOrder = 10; // Set sorting order for upgrade text
        currencyTextComponent.sortingOrder = 10; // Set sorting order for currency text
        currencyIconComponent.sortingOrder = 10; // Set sorting order for currency icon
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Upgrade Acquired.");
            //string upgradeType = upgrade.UpgradeType;
            //float upgradeAmount = upgrade.UpgradeValue;
            if (isBaseUpgrade)
            {
                switch (upgradeType)
                {
                    case "hp":
                        baseStats.Max_Health += upgradeAmount;
                        baseStats.Current_Health = baseStats.Max_Health;
                        break;
                    case "jump":
                        baseStats.JumpPower += upgradeAmount; break;
                    case "dodge":
                        baseStats.Dodges += 1;
                        break;
                    case "damage":
                        baseStats.Damage += upgradeAmount; break;
                }
            }
            else
            {
                switch (upgradeType)
                {
                    case "hp":
                        playerStats.Max_Health += upgradeAmount;
                        playerStats.Current_Health = playerStats.Max_Health;
                        break;
                    case "jump":
                        playerStats.JumpPower += upgradeAmount; break;
                    case "dodge":
                        playerStats.Dodges += 1;
                        break;
                    case "damage":
                        playerStats.Damage += upgradeAmount; break;
                }
            }
            Destroy(gameObject);
        }
    }
    */

    private int getUpgradeTextIndex()
    {
        int index = 0;
        if (isBaseUpgrade)
        {
            index += 4; // Base upgrades start at index 4
        }
        switch (upgradeType)
        {
            case "hp":
                index += 0; // Health upgrades
                break;
            case "damage":
                index += 1; // Damage upgrades
                break;
            case "jump":
                index += 2; // Jump upgrades
                break;
            case "dodge":
                index += 3; // Dodge upgrades
                break;
            default:
                UnityEngine.Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                return -1; // Return -1 for unknown upgrade types
                //return "Unknown Upgrade";
        }
        return index;
    }

        void makeDescriptions()
    {
        upgradeDescriptions.Add("[E] \nIncreases maximum health by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \nIncreases damage by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \nIncreases jump height by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \nIncreases maximum dodges by 1");
        upgradeDescriptions.Add("[E] \n[Eternal] Permanantly increases maximum health by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \n[Eternal] Permanantly increases damage by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \n[Eternal] Permanantly increases jump height by " + upgradeAmount.ToString());
        upgradeDescriptions.Add("[E] \n[Eternal] Permanantly increases maximum dodges by 1");
    }
}
