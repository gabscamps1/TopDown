using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject enemyFighter;
    [SerializeField] int amountEnemyFighter;

    [SerializeField] GameObject enemyShooter;
    [SerializeField] int amountEnemyShooter;

    GameObject[] enemys;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemys.Length > 0 || enemyFighter == null || enemyShooter == null) return;

        if (amountEnemyFighter > 0)
        {
            Instantiate(enemyFighter, transform.position, Quaternion.identity);
            amountEnemyFighter--;
        }
        else if (amountEnemyShooter > 0)
        {
            Instantiate(enemyShooter, transform.position, Quaternion.identity);
            amountEnemyShooter--;
        }
    }
}
