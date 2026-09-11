using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Objects", order = 1)]
public class Player_Stats : ScriptableObject
{
    //Max Health
    [SerializeField]
    private float max_Health;
    public float Max_Health {get {return max_Health;} set {max_Health = value;} }

    //current Health
    [SerializeField] private float current_Health;
    public float Current_Health
    {
        get { return current_Health; }
        set 
        {
            if (value <= max_Health) //current health will not exceed max health
            {
                current_Health = value;
            }
            else
            {
                current_Health = max_Health;
            }
        }
    }

    //Money
    //Only have two currencies, bone dust and gold coins
    [SerializeField]
    private int bonedust;
    public int Bonedust
    {
        get { return bonedust; }
        set
        {
            if (value >= 0) //bonedust cannot be negative
            {
                bonedust = value;
            }
            else
            {
                bonedust = 0;
            }
        }
    }
    [SerializeField]
    private int goldcoins;
    public int Goldcoins
    {
        get { return goldcoins; }
        set
        {
            if (value >= 0) //goldcoins cannot be negative
            {
                goldcoins = value;
            }
            else
            {
                goldcoins = 0;
            }
        }
    }
    
    //Speed
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    //Jump Power
    [SerializeField]
    private float jumpPower;
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }

    //Dammage
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    public float Dodges;
    public float Max_Dodges;

    [SerializeField]
    private int current_Loop = 0; 

    //start one 0
        //0 unlocks phase one
        //1 unlocks a extra attack to phase one
        //2 unlocks phase two
    public int Current_loop //Start loop on zero
    {
        get { return current_Loop; }
        set
        {
            if (value < 3)
            {
                current_Loop = value;

            }
            else
            {
                current_Loop = 2;
            }
        }

    }

}
