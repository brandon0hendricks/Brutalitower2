using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrades", order = 1)]

public class UpgradeScriptableObject : ScriptableObject
{
    [SerializeField]
    public string UpgradeType { get; set; }
    [SerializeField]
    public float UpgradeValue { set; get; }
    public bool IsPurchasable { set; get;}
    [SerializeField]
    private int upgradePrice;
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

}
