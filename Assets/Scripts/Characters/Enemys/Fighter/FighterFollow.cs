using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FighterFollow : MonoBehaviour
{
    GameObject player; // Referêmcia do Player.
    NavMeshAgent agent; // Referêmcia do NavMeshAgent.
    Animator animator; // Referêmcia do Animator.
    [SerializeField] float moveSpeed; // Velocidade do Inimigo
    [SerializeField] float stopSpeed; // Velocidade do Inimigo recarregando o ataque.
    [SerializeField] Collider2D damageArea; // Área que o Inimigo pode causar dano ao Player.
    bool canCauseDamage;
    bool stopEnemy;
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
        if (stopEnemy) return;

        CalculateAction();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }

        if (collision.CompareTag("Player") && collision.IsTouching(damageArea))
        {
            canCauseDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.IsTouching(damageArea))
        {
            canCauseDamage = false;
        }
    }

    void CalculateAction()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        LayerMask layermask = LayerMask.GetMask("Player") | LayerMask.GetMask("SceneObject"); // Layers para serem detectadas no raycast.
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, 1.3f, layermask);
        Debug.DrawLine(transform.position + (Vector3.up * 0.6f), hit.point);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Para o movimento do Inimigo.
                agent.ResetPath();

                // Para a animação de Walk.
                animator.SetBool("Walk", false);

                // Inicia a animação de Ataque.
                animator.SetTrigger("Attack");

                // Tempo de espera entre os ataque de perto do Inimigo.
                StartCoroutine(WaitToMove());
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

    // Função para seguir o Player.
    void FollowPlayer()
    {
        // SubBoss se move até o Player.
        agent.SetDestination(player.transform.position);

        // Seta a animação de andar para true.
        animator.SetBool("Walk", true);
    }

    // Função de ataque. Chamada na animação de ataque do Inimigo.
    void Attack(string direction)
    {
        // Pega posição do Player em relação ao Inimigo.
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        // Pega a direção do ataque do Inimigo.
        Vector2 attackDirection = Vector2.zero;
        switch (direction)
        {
            case "Up":
                attackDirection = Vector2.up;
                break;
            case "Down":
                attackDirection = Vector2.down;
                break;
            case "Right":
                attackDirection = transform.right;
                break;
        }

        // Calcula o dotProduct da direção do player em relação a direção de ataque do Inimigo.
        float dotProduct = Vector3.Dot(attackDirection, playerDirection);
        if (dotProduct >= 0.4 && canCauseDamage)
        {
            PlayerDamage playerScript = player.GetComponent<PlayerDamage>();
            playerScript.CallDamage(1);
        }

        // Finaliza a animação de ataque.
        animator.ResetTrigger("Attack");
    }

    IEnumerator WaitToMove()
    {
        stopEnemy = true;

        yield return new WaitForSeconds(stopSpeed);

        stopEnemy = false;
    }
}
