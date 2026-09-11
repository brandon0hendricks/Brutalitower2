using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Now defunct, currently using CurrencyCount.cs for currency display
public class MoneyCounter : MonoBehaviour
{
    TMPro.TMP_Text text;
    [SerializeField]
    private Player_Stats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent <TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = playerStats.Money.ToString();
    }
}
