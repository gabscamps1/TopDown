using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    bool isFogActive = true;
    [SerializeField] BoxCollider2D areaFog;
    Collider2D[] objects;

    void Start()
    {
        objects = Physics2D.OverlapBoxAll(transform.position, areaFog.size, 0);
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Enemy"))
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {
       if (!isFogActive)
        {
            foreach (var obj in objects)
            {
                obj.gameObject.SetActive(true);   
            }
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFogActive = false;
        }
    }
}
