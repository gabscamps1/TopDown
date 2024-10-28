using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FighterFollow : MonoBehaviour
{
    GameObject player; // Referêmcia do Player.
    NavMeshAgent agent; // Referêmcia do NavMeshAgent.
    Animator animator; // Referêmcia do Animator.
    [SerializeField] float moveSpeed; // Velocidade do Inimigo
    [SerializeField] float stopSpeed; // Velocidade do Inimigo recarregando.

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("PlayerChildren"); // Layers para não serem detectadas no raycast.
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, 0.4f, ~ignoreLayermask);
        Debug.DrawLine(transform.position + (Vector3.up * 0.6f), hit.point);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.ResetPath();
                animator.SetBool("Walk", false);
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            FollowPlayer();
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
        animator.SetBool("Walk", true);
    }
}
