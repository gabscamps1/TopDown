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
        // Chama a fun��o de abrir a porta quando o Player entra em sua �rea. 
        if (collision.CompareTag("Player"))
        {
            OpenTrapDoor();
        }
    }

    void OpenTrapDoor()
    {
        // Toca a anima��o do al�ap�o abrindo.
        Animator doorAnimator = GetComponentInChildren<Animator>();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
            this.enabled = false;
        }
    }
}
