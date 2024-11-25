using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SubBoss_Attack : MonoBehaviour
{
    GameObject player; // Referência do Player.
    NavMeshAgent agent; // Referência do NavMeshAgent.
    Animator animator; // Referência do Animator.
    GunsEnemy gunScript; // Referência do Script da arma.

    [Header("InfoEnemy")]
    [SerializeField] float moveSpeed; // Velocidade do SubBoss.

    [Header("CloseRangeAttack")]
    bool closeRangeAttack; // Confere se o SubBoss ira atacar de perto.
    [SerializeField] Collider2D damageArea; // área que o SubBoss pode causar dano ao Player.
    bool canCauseDamage;
    bool stopEnemy;
    [SerializeField] float stopSpeed; // Velocidade do SubBoss recarregando o ataque.
    [SerializeField] float timerCloseAttack; // Tempo para o SubBoss realizar o ataque de perto, caso não consiga, volta para o ataque a distância.


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        animator = GetComponent<Animator>();

        gunScript = GetComponentInChildren<GunsEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        // Se não tiver player na área do SubBoss ou o SubBoos não estiver com arma, retorna a função.
        if (player == null || gunScript == null) return;

        // Confere se está durante o cooldown do ataque de perto.
        if (stopEnemy) return;

        // Confere se ira atacar de longe ou de perto.
        if (closeRangeAttack)
        {
            // Chama a função de ataque de perto.
            CloseRangeAttack();
        }
        else
        {
            // Chama a função de ataque a distância.
            LongeRangeAttack();
        }
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Player entrou na área de visão do SubBoss.
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }


        // Player entrou na área de dano de perto do SubBoss.
        if (collision.CompareTag("Player") && collision.IsTouching(damageArea))
        {
            canCauseDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player saiu da área de dano de perto do SubBoss.
        if (collision.CompareTag("Player") && !collision.IsTouching(damageArea))
        {
            canCauseDamage = false;
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

    void PickGun()
    {
        // Ativa a arma.
        gunScript.gameObject.SetActive(true);

        // Recarrega as balas da arma instantaneamente.
        gunScript.currentAmmo = gunScript.maxAmmo;
    }

    // Função para o ataque de longe.
    void LongeRangeAttack()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        // LayerMask layermask = LayerMask.GetMask("Player") | LayerMask.GetMask("SceneObject") | LayerMask.GetMask("Invulnerability"); // Layers para serem detectadas no raycast.
        LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, 16, ~ignoreLayermask);
        Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point);

        if (!gunScript.isReloading)
        {
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
        else
        {
            closeRangeAttack = true;
        }
    }

    // Função para o ataque de perto.
    void CloseRangeAttack()
    {
        // Desativa a arma.
        gunScript.gameObject.SetActive(false);

        // Inicia a coroutine que calcula o tempo que o SubBoss tem para atacar.
        StartCoroutine(TimerCloseAttack());

        Vector2 direction = (player.transform.position - transform.position).normalized;
        LayerMask layermask = LayerMask.GetMask("Player") | LayerMask.GetMask("SceneObject"); // Layers para serem detectadas no raycast.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, 1.3f, layermask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Para o movimento do SubBoss.
                agent.ResetPath();

                // Para a animação de Walk.
                animator.SetBool("Walk", false);

                // Inicia a animação de Ataque.
                animator.SetTrigger("Attack");

                // Tempo de espera após o ataque de perto do SubBoss, para iniciar o ataque à distância.
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

    // Função de ataque. Chamada na animação de ataque do SubBoss.
    void Attack(string direction)
    {
        // Pega posição do Player em relação ao SubBoss.
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        // Pega a direção do ataque do SubBoss.
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

        // Calcula o dotProduct da direção do player em relação a direção de ataque do SubBoss.
        float dotProduct = Vector3.Dot(attackDirection, playerDirection);
        if (dotProduct >= 0.4 && canCauseDamage)
        {
            PlayerDamage playerScript = player.GetComponent<PlayerDamage>();
            playerScript.CallDamage(1);
        }

        // Finaliza a animação de ataque.
        animator.ResetTrigger("Attack");
        
    }

    // Função que calcula o tempo de espera após o ataque de perto do SubBoss, para iniciar o ataque à distância.
    IEnumerator WaitToMove()
    {
        stopEnemy = true;

        yield return new WaitForSeconds(stopSpeed);

        stopEnemy = false;

        // Termina o ataque de perto.
        closeRangeAttack = false;

        // Chama a função de puxar a arma.
        PickGun();
    }

    // Função que calcula o tempo que o SubBoss tem para atacar.
    IEnumerator TimerCloseAttack()
    {
        yield return new WaitForSeconds(timerCloseAttack);

        // Termina o ataque de perto.
        closeRangeAttack = false;

        // Chama a função de puxar a arma.
        PickGun();
    }


}