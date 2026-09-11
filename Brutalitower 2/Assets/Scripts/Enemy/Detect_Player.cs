using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Health_bar;
    [SerializeField] private SpriteRenderer Health_fill;

    private Vector2 enemyLocation;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Dispay_Health();
    }

    void Dispay_Health()
    {
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if(3.5f > Mathf.Abs(distance))
        {
            Health_bar.enabled = true;
            Health_fill.enabled = true;
        }
        else
        {
            Health_bar.enabled = false;
            Health_fill.enabled = false;
        }
        
    }
}
