using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedDoors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Chama a função de abrir a porta quando o Player entra em sua área. 
        if (collision.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Transform doorTransform = transform.GetChild(0);
        Collider2D doorCollider = doorTransform.GetComponent<Collider2D>();
        // Tira a colisão da porta quando aberta.
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
            


    }
}
