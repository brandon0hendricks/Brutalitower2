using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    public float width, height;
    [SerializeField]
    private Player_Stats playerStats;

    [SerializeField]
    private RectTransform healthBar;

    public void changeHealth(float health, float maxHealth)
    {
        float newWidth = (health / maxHealth)*width;
        healthBar.sizeDelta = new Vector2(newWidth, height);
    }

}
