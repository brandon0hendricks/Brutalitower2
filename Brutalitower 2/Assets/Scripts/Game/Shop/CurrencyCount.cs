using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class CurrencyCount : MonoBehaviour
{
    [SerializeField]
    private Player_Stats playerStats;
    [SerializeField]
    TMPro.TMP_Text bonedusttext;
    [SerializeField]
    TMPro.TMP_Text goldcointext;
    public int BonedustCount;
    public int GoldcoinCount;

    void Update()
    {
        BonedustCount = playerStats.Bonedust; // Initialize BonedustCount from Player_Stats
        GoldcoinCount = playerStats.Goldcoins; // Initialize GoldcoinCount from Player_Stats
        UpdateBonedustCount(); // Update the UI text for Bone Dust
        UpdateGoldcoinCount(); // Update the UI text for Gold Coins
    }
    void Awake()
    {
        //bonedusttext = GetComponent<TMPro.TMP_Text>();
        //goldcointext = GetComponent<TMPro.TMP_Text>();
        BonedustCount = 0;
        GoldcoinCount = 0;

       
    }

    void OnEnable() => Currency.OnCollected += OnCollected;
    void OnDisable() => Currency.OnCollected -= OnCollected;

    void OnCollected(Currency currency)
    {
        if(currency.IsBoneDust)
        {
            BonedustCount += currency.CurrencyValue;
            playerStats.Bonedust += currency.CurrencyValue; // Update Player_Stats with the collected Bone Dust
            UpdateBonedustCount();
        }
        else if (currency.IsGoldCoin)
        {
            GoldcoinCount += currency.CurrencyValue;
            playerStats.Goldcoins += currency.CurrencyValue; // Update Player_Stats with the collected Gold Coins
            UpdateGoldcoinCount();
        }
    }

    void UpdateBonedustCount()
    {
        bonedusttext.text = (BonedustCount.ToString());
    }
    void UpdateGoldcoinCount()
    {
        goldcointext.text = (GoldcoinCount.ToString());
    }
    /*
    void OnGoldcoinCollected()
    {
        text.text = "Bonedust: " + (bonedustCount.ToString()) + "\nGold Coins: " + (goldcoinCount.ToString());
    }
    */
}
