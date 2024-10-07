using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderPosition : MonoBehaviour
{
    [SerializeField] Animator animator;
    Transform player;
    PlayerMovement playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Transform>();
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (animator != null)
        {
            if (animator.GetBool("Right") == true)
            {
                transform.position = new Vector3 (player.transform.position.x + 0.03f, transform.position.y - 0.28f, 0);
            }
            *//*else if (animator.GetBool("Left") == true)
            {
                transform.position = new Vector3(player.transform.position.x - 0.03f, transform.position.y - 0.28f, 0);
            }*//*

            if (animator.GetBool("Up") == true)
            {
                if(playerScript.dotProductRight > 0) transform.position = new Vector3(player.transform.position.x + 0.24f, transform.position.y - 0.28f, 0);
                else transform.position = new Vector3(player.transform.position.x - 0.24f, transform.position.y - 0.28f, 0);

            }
            else if (animator.GetBool("Down") == true)
            {
                if (playerScript.dotProductRight > 0)
                {
                    transform.position = new Vector3(player.transform.position.x + 0.24f, transform.position.y - 0.28f, 0);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x - 0.24f, transform.position.y - 0.28f, 0);
                }
            }
        }
    }*/
}
