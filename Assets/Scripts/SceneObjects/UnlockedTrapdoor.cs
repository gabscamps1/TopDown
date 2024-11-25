using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnlockedTrapDoors : MonoBehaviour
{

    private void Start()
    {

    }
    void OnTriggerStay2D(Collider2D collision)
    {
        // Chama a função de abrir a porta quando o Player entra em sua área. 
        if (collision.CompareTag("Player"))
        {
            OpenTrapDoor();
        }
    }

    void OpenTrapDoor()
    {
        // Toca a animação do alçapão abrindo.
        Animator doorAnimator = GetComponentInChildren<Animator>();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
            this.enabled = false;
        }
    }
}
