using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public Sprite CurrencySprite; // Reference to the sprite for the currency, can be set in the inspector
    private int currencyValue; // Value of the currency, can be set in the inspector
    private bool isBoneDust; // Flag to indicate if this is a Bone Dust currency
    private bool isGoldCoin; // Flag to indicate if this is a Gold Coin currency

    public bool IsBoneDust
    {
        get { return isBoneDust; }
        set { isBoneDust = value; }
    }
    public bool IsGoldCoin
    {
        get { return isGoldCoin; }
        set { isGoldCoin = value; }
    }

    public int CurrencyValue
    {
        get { return currencyValue; }
        set { currencyValue = value; }
    }

    public Currency()
    {
        // Default constructor
        currencyValue = 100; // Default value, can be overridden in the inspector
        isBoneDust = true; // Default to Bone Dust
        isGoldCoin = false; // Default to not Gold Coin
    }

    public Currency(int value, bool boneDust, bool goldCoin)
    {
        currencyValue = value;
        isBoneDust = boneDust;
        isGoldCoin = goldCoin;
    }

    // Event to notify when currency is collected
    public static event Action<Currency> OnCollected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                OnCollected?.Invoke(this);
                Destroy(gameObject);
        }
    }
}
