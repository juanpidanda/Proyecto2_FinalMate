using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    PlayerHealth PlayerHealth;

    public int damage;
    public float damageTime;
    float CurrentDamage;

    float bulletLifeTime;
    void Start()
    {
        bulletLifeTime = 0.0f;
        PlayerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            PlayerHealth.health += damage;
            Destroy(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        bulletLifeTime += Time.deltaTime;
        if (bulletLifeTime >= 4.0f)
        {
            Destroy(gameObject);
        }
    }
}
