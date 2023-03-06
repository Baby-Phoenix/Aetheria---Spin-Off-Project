using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameobjects : MonoBehaviour
{
    GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        print(enemies.Length);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {


            // Set each enemy as active
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
        }
    }

}
