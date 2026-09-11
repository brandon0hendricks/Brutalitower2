using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_attack : MonoBehaviour
{
    //Psedocode
    // 1) Find location player is closest to
    // 2) teleport to location(play animation)
    // 3) Attack in desired location
    // 4) wait a cooldown time
    // 5) repeat

    //Get locations for transportation
    private Transform[] Attack_locations;
    private Find_Closest_Location Find_Locations;
    private Transform next_location;

    [SerializeField] private enemySpawner spawner;
    [SerializeField] private Transform[] location;

    private Boss_Controller boss_Controller;
    [SerializeField] private Collider2D AttackBox;
    [SerializeField] private Collider2D R_AttackBox;

    // Start is called before the first frame update
    void Start()
    {
        boss_Controller = GetComponent<Boss_Controller>();  
        Attack_locations = Game_Manager.instance.BossRoomLocations;//Get locations boss can attack
        //Locations values for boss
        //0 left
        //1 center
        //2 Right

        Find_Locations = gameObject.GetComponent<Find_Closest_Location>(); //Get script Attached to boss to find player area

    }
    

    void Next_Location() //Finds next attack location
    {
        switch(Find_Locations.FindClosestLocation())
        {
            case "left":
                transform.position = Attack_locations[0].position;
                boss_Controller.Flip_boss_Direction(1); //face right
                break;
            case "Center":
                transform.position = Attack_locations[1].position;
                boss_Controller.Flip_To_Player(); //flip to face right
                break;
            case "Right":
                transform.position = Attack_locations[2].position;
                boss_Controller.Flip_boss_Direction(-1); //face left
                break;
            default:
                transform.position = Attack_locations[0].position;
                break;
        }     
    }

    void Start_Teleport()
    {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(boss_Controller.base_stats.Attack_cooldown); //wait cooldown after attacking
        boss_Controller.animator.SetTrigger("Teleport");
    }

    void choose_Attack() //choose next attack
    {
        int next_attack = 1;
        if(boss_Controller.current_Phase >= 1)
        {
            Debug.Log("Choosing value");
            next_attack = Random.Range(1, boss_Controller.current_Phase + 2);
            Debug.Log(next_attack);
        }
        switch(next_attack)
        {
            case 1:
                Side_Attack();
                break;
            case 2:
                int randomLocation = Random.Range(1, 3);
                if(randomLocation == 1)
                {
                    transform.position = transform.position = Attack_locations[0].position;
                    boss_Controller.Flip_boss_Direction(1); //face right
                }
                else         
                {
                    transform.position = transform.position = Attack_locations[2].position;
                    boss_Controller.Flip_boss_Direction(-1); //face left
                }
                Ranged();
                break;
            case 3:
                spawn_charger();
                break;
        }
    }
      
    void Side_Attack()
    {
        Debug.Log("attack");
        boss_Controller.animator.SetTrigger("S_Attack"); //Start attack
    }
    void Ranged()
    {
        Debug.Log("ranged attack");
        boss_Controller.animator.SetTrigger("R_Attack"); //Start attack
    }

    void spawn_charger()
    {
        //spawn enemy at specific location
        spawner.Spawn(enemyController.EnemyTypes.Charger, location[0].position);
        boss_Controller.animator.SetTrigger("Spawn"); //Start attack
        Start_Teleport();
    }


    //Functions for activating boss damage and disabiling
    void Enable_Damage()
    {
        AttackBox.enabled = true;

    }
    void disable_damage()
    {
        AttackBox.enabled = false;
    }

    void R_Enable_Damage()
    {
        R_AttackBox.enabled = true;

    }
    void R_disable_damage()
    {
        R_AttackBox.enabled = false;
    }

}
