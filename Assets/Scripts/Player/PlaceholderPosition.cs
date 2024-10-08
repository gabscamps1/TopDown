using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderPosition : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    // PlayerMovement playerScript;
    AnimatorStateInfo playerState;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null && animator != null)
        {
            playerState = animator.GetCurrentAnimatorStateInfo(0);
            if (playerState.IsName("Right") || playerState.IsName("Up"))
            {
                spriteRenderer.sortingOrder = 2;
            }
            else if (playerState.IsName("Down"))
            {
                spriteRenderer.sortingOrder = 0;
            }
        }
        

        // Não apagar função abaixo - Teste futuro

        /*if (playerState.IsName("Left"))
        {
            transform.localPosition = new Vector3(-0.24f, -0.28f, 0);
            player.sortingOrder = 2;
        }
        else if (playerState.IsName("Right"))
        {
            transform.localPosition = new Vector3(0.24f, -0.28f, 0);
            player.sortingOrder = 2;
        }
        else if (playerState.IsName("Up")) 
        {
            player.sortingOrder = 2;
            if (playerScript.dotProductRight > 0)
            {
                transform.localPosition = new Vector3(0.24f, -0.28f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(-0.24f,  -0.28f, 0);
            }
        }
        else if (playerState.IsName("Down"))
        {
            player.sortingOrder = 0;
            if (playerScript.dotProductRight > 0)
            {
                transform.localPosition = new Vector3(0.24f, -0.28f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(-0.24f, -0.28f, 0);
            }
        }*/
    }
}


