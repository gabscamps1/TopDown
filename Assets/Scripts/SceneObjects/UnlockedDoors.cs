using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedDoors : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D collision)
    {
        // Chama a fun��o de abrir a porta quando o Player entra em sua �rea. 
        if (collision.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Transform doorTransform = transform.GetChild(0);
        Collider2D doorCollider = doorTransform.GetComponent<Collider2D>();
        // Tira a colis�o da porta quando aberta.
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
            


    }
}
