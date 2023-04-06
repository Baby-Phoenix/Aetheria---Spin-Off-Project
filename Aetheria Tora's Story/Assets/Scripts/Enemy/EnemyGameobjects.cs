using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyGameobjects : MonoBehaviour
{
    public PlayerStats playerStats;

    GameObject[] enemies;

    public Transform respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Find all game objects with the tag "enemy"
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        if (playerStats.currentHealth <= 0)
        {
            // Set each enemy as active
            foreach (GameObject enemy in enemies)
            {
                enemy.transform.position = enemy.GetComponent<EnemyStats>().originalPos;
                enemy.GetComponent<EnemyManager>().currentTarget = null;
                enemy.GetComponent<EnemyManager>().currentState = enemy.GetComponent<EnemyManager>().idleState;
                enemy.GetComponent<EnemyStats>().currentHealth = enemy.GetComponent<EnemyStats>().maxHealth;
                enemy.GetComponent<EnemyStats>().healthBar.SetMaxHealth(enemy.GetComponent<EnemyStats>().maxHealth);
                enemy.GetComponent<EnemyStats>().isDead = false;
                enemy.SetActive(true);
            }
            playerStats.gameObject.SetActive(false);
            playerStats.gameObject.GetComponent<Transform>().position = respawnPoint.position;
            playerStats.currentHealth = playerStats.maxHealth;
            playerStats.healthBar.SetMaxHealth(playerStats.maxHealth);
            playerStats.gameObject.SetActive(true);
        }
    }
}







