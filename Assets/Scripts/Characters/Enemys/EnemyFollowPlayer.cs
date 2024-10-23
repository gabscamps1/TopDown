using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("PlayerChildren"); // Layers para não serem detectadas no raycast.
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, direction, 8, ~ignoreLayermask);
            Debug.DrawLine(transform.position, hit.point);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    agent.SetDestination(transform.position);
                }
                else
                {
                    agent.SetDestination(player.transform.position);
                    print("oi");
                }
            }
            else
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

}
