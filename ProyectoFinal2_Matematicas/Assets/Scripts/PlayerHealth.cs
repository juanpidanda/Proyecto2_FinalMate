using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public Image HealthBar;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, 100);
        HealthBar.fillAmount = health / 100;
    }
}
