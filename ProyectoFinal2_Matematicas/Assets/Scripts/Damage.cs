using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    PlayerHealth PlayerHealth;

    public int damage;
    public float damageTime;
    float CurrentDamage;
    void Start()
    {
        PlayerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            PlayerHealth.health += damage;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
