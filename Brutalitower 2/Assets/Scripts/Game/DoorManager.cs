using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    // This script manages doors

    private SpriteRenderer sprite;
    [SerializeField] private Sprite doorOpen;
    [SerializeField] private Sprite doorClosed;
    [SerializeField] private Transform playerDetector;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform teleportLocation;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject doorText;
    [SerializeField] private GameObject fader;

    [SerializeField] private int door_checkpoint;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();   
    }

    private bool PlayerProximity()
    {
        return Physics2D.OverlapCircle(playerDetector.position, 0.4f, playerLayer);
    }

    void Update()
    {
        if (PlayerProximity())
        {
          
            doorText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (door_checkpoint == 0)
                {
                    StartCoroutine(TeleportPlayer());
                }
                else if (door_checkpoint == 1)
                {
                    if(Game_Manager.instance.Current_Stats.Current_loop == 0)
                    {
                        Game_Manager.instance.leave_Scene("BossRoom");
                    }
                    else
                    {
                        StartCoroutine(TeleportPlayer());
                    }

                }
                else if (door_checkpoint == 2)
                {
                    if(Game_Manager.instance.Current_Stats.Current_loop == 1)
                    {
                        Game_Manager.instance.leave_Scene("BossRoom");
                    }
                    else
                    {
                        StartCoroutine(TeleportPlayer());
                    }
                }
                else if(door_checkpoint == 3)
                {
                    if (Game_Manager.instance.Current_Stats.Current_loop == 2)
                    {
                        Game_Manager.instance.leave_Scene("BossRoom");
                    }
                    else
                    {
                        StartCoroutine(TeleportPlayer());
                    }
                }
            }
        }
        else
        {
            doorText.SetActive(false);
        }
    }

    private IEnumerator TeleportPlayer()
    {
        sprite.sprite = doorOpen;
        fader.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        player.transform.position = teleportLocation.position;
        sprite.sprite = doorClosed;
    }
}
