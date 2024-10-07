using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderPosition : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer player;
    PlayerMovement playerScript;
    AnimatorStateInfo playerState;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        playerState = animator.GetCurrentAnimatorStateInfo(0);

        if (playerState.IsName("Left"))
        {
            transform.localPosition = new Vector3(-0.03f, -0.28f, 0);
            player.sortingOrder = 2;
        }
        else if (playerState.IsName("Right"))
        {
            transform.localPosition = new Vector3(0.03f, -0.28f, 0);
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
        }
    }
}


