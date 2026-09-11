using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingFlickering : MonoBehaviour
{
    // This is just a test. Feel free to remove.

    [SerializeField] private Light2D light2D;
    [SerializeField] private float timerMax = 0.5f;
    private float timer;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    private float timerMin = 0f;
    [SerializeField] private float frequency;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void FixedUpdate()
    {
        light2D.intensity = Mathf.PerlinNoise(timerMin, timer * frequency) * (maxIntensity - minIntensity) + minIntensity;
        timer += Random.Range(0.01f, timerMax);
    }
}

