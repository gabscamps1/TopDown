using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnlockedDoors : MonoBehaviour
{

    private void Start()
    {
        
    }
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

        // Tira a porta como obst�culo.
        NavMeshObstacle doorObstacle = doorTransform.GetComponent<NavMeshObstacle>();

        if (doorObstacle != null)
        {
            doorObstacle.enabled = false;
        }

        // Tira a colis�o da porta quando aberta.
        Collider2D doorCollider = doorTransform.GetComponent<Collider2D>();

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // Toca a anima��o da porta abrindo.
        Animator doorAnimator = GetComponentInChildren<Animator>();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
            this.enabled = false;
        }

    }
}
