using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Boss_Stats : MonoBehaviour
{

    //This SCRIPT ONLY GIVES THE STARTING BASE STATS
    //Boss Base Stats
    [SerializeField] private float health; //Max Health Upon starting
    public float Health {  get { return health * SetModifier(); } set { } }

    [SerializeField] private float attack_cooldown; //base cooldown between attacks
    public float Attack_cooldown { get {return attack_cooldown / SetModifier(); } set { } }

    [SerializeField] private float damage; //base damage
    public float Damage { get { return damage * SetModifier(); ; } set { } }

    
    private float SetModifier() //Set Stats Based off of stat modifier
    {
        switch (Game_Manager.instance.Current_Stats.Current_loop)
        {
            case 0:
                return 1f;
            case 1:
                return 1.5f;
            case 2:
                return 2f;
            default:
                return 1f;
        }
    }
}
