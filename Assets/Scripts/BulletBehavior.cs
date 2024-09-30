using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BulletBehavior : MonoBehaviour
{
    public float timeToDespawn = 2f;

    void Start()
    {
        Destroy(gameObject, timeToDespawn);

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }


    // private void OnParticleCollision(GameObject collision){
    //         if (collision.CompareTag("Enemy"))
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
}
