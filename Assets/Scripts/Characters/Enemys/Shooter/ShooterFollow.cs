using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterFollow : MonoBehaviour
{
    GameObject player; // Refer�mcia do Player.
    NavMeshAgent agent; // Refer�mcia do NavMeshAgent.
    Animator animator; // Refer�mcia do Animator.
    GunsEnemy gunScript; // Refer�mcia do Script da arma.
    bool canStartCoroutine; // Conferir se pode iniciar coroutine.
    [SerializeField] float moveSpeed; // Velocidade do Inimigo
    [SerializeField] float sideSpeed; // Velocidade do Inimigo recarregando.
    LayerMask layermask;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        animator = GetComponent<Animator>();

        gunScript = GetComponentInChildren<GunsEnemy>();

        layermask = LayerMask.GetMask("Player") | LayerMask.GetMask("SceneObjects") | LayerMask.GetMask("Invulnerability"); // Layers para serem detectadas no raycast.
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || gunScript == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, 8, layermask);

        if (!gunScript.isReloading)
        {
            canStartCoroutine = true;

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                agent.ResetPath();
                animator.SetBool("Walk", false);
            }
            else
            {
                FollowPlayer();
            }

        }
        else if (canStartCoroutine == true)
        {
            StartCoroutine(WalkSideways(direction));
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

    IEnumerator WalkSideways(Vector2 direction)
    {
        canStartCoroutine = false; // Impede de chamar a coroutine novamente durante sua realiza��o.

        agent.ResetPath();

        Vector3 perpendicularDirection = new Vector2(-direction.y,direction.x).normalized;

        // Move para o lado por um tempo definido.
        float elapsedTime = 0f;
        float duration = gunScript.reloadTime/4; // Dura��o da movimenta��o para um lado � a metade do tempo de recarga.

        animator.SetBool("Walk", true);

        while (gunScript.isReloading && elapsedTime < duration)
        {
            // Atualiza a posi��o continuamente.
            transform.position += perpendicularDirection * sideSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime; // Incrementa o tempo passado.
            yield return null; // Espera at� o pr�ximo quadro.
        }

        // Volta para a posi��o original.
        elapsedTime = 0f;

        while (gunScript.isReloading && elapsedTime < duration*2)
        {
            // Atualiza a posi��o de volta
            transform.position -= perpendicularDirection * sideSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime; // Incrementa o tempo passado.
            yield return null; // Espera at� o pr�ximo quadro.
        }

        // Volta para a posi��o original.
        elapsedTime = 0f;

        while (gunScript.isReloading && elapsedTime < duration)
        {
            // Atualiza a posi��o continuamente.
            transform.position += perpendicularDirection * sideSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime; // Incrementa o tempo passado.
            yield return null; // Espera at� o pr�ximo quadro.
        }

        animator.SetBool("Walk", false);
        
    }

}
