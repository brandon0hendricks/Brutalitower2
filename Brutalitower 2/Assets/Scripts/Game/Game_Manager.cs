using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
public class Game_Manager : MonoBehaviour
{

    [SerializeField] private Player_Stats playerStats;

    //Health Stats
    [SerializeField] private Image healthBar;
    float shown_health;

    [SerializeField] private Image staminaBar;
    float shown_Stamina;

    public static Game_Manager instance;
    public GameObject player;

    public Player_Stats Current_Stats; //This is the current Round stats
    [SerializeField] private Player_Stats base_Stats;// This is the overall stats


    [SerializeField] private float shakeForce = .5f; //To shake Camera
    [SerializeField] private Animator game_fade;


    //Boss room locations
    public Transform[] BossRoomLocations;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Game_Area")
        {
            Debug.Log("reset Stats");
            Set_Player_Stats();
        }
    }

    private void FixedUpdate()
    {
        changeHealth();
        ChangeStamina();
    }


    public void Camera_Shake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }

    void Set_Player_Stats()
    {
        Current_Stats.Current_Health = base_Stats.Max_Health;
        //Set all stats to base on game start
        Current_Stats.Max_Health = base_Stats.Max_Health;
        Current_Stats.Speed = base_Stats.Speed;
        Current_Stats.JumpPower = base_Stats.JumpPower;
        Current_Stats.Damage = base_Stats.Damage;
        Current_Stats.Dodges = base_Stats.Max_Dodges;
        Current_Stats.Max_Dodges = base_Stats.Max_Dodges;
        
    }


    public void changeHealth()
    {
        healthBar.fillAmount = Mathf.Lerp(shown_health, Current_Stats.Current_Health / Current_Stats.Max_Health, .2f);
        shown_health = healthBar.fillAmount;
    }
    public void ChangeStamina()
    {
        staminaBar.fillAmount = Mathf.Lerp(shown_Stamina, Current_Stats.Dodges / Current_Stats.Max_Dodges, .2f);
        shown_Stamina = staminaBar.fillAmount;  
    }

    public void leave_Scene(string scene) // this function will set player back to intro
    {
        game_fade.SetTrigger("Fade");
        StartCoroutine(wait_to_load(scene));
    }
     private IEnumerator wait_to_load(string scene)
    {
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(scene); //Load boss room

    }

}
