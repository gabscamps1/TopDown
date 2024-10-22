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
        if (player != null)
        {
            

            if (Mathf.Abs(transform.position.x - player.transform.position.x) < agent.stoppingDistance)
            {
                // KeepDistanceFromPlayer();
            }
            else
            {
                FollowPlayer();
            }
        }

        
    }

    private void FixedUpdate()
    {
        LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("PlayerChildren"); // Layers para não serem detectadas no raycast.
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.right, transform.right, Mathf.Infinity,~ignoreLayermask);
        Debug.DrawLine(transform.right, hit.point);
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

    void KeepDistanceFromPlayer()
    {
        Vector2 direction = (transform.position - player.transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + (direction * agent.stoppingDistance);
        agent.SetDestination(newPosition);
    }
}
