using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bishopBalls : MonoBehaviour
{
    [SerializeField] private float vanishTime;
    [SerializeField] private float vanishSpeed;
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        enabled = false;
        StartCoroutine(Vanish());
    }
    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(vanishTime);
        enabled = true;
    }

    void Update()
    {
        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, 0, vanishSpeed);
        sprite.color = c;
        if (c.a <= 0.01f)
        {
            Destroy(gameObject);
        }   
    }
}
