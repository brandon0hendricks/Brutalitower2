using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Enemies/Enemy Stats SO")]

public class enemyStats : ScriptableObject
{
    //This SO stores the stats for the enemies!

    public float speed;
    public float chaseDistance;
    public float jumpHeight;
    public float health;
    public float cooldown;
    public RuntimeAnimatorController animatorController;
}
