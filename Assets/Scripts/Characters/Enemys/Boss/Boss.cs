using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent; // Refer�mcia do NavMeshAgent.
    Animator animator; // Refer�mcia do Animator.
    GunsEnemy gunScript;
    LayerMask ignoreLayermask;
    [SerializeField] float moveSpeed; // Velocidade do Boss
    [SerializeField] float timeToBossRun;
    [SerializeField] GameObject[] localPoints; // Pontos no cen�rio em que o Boss caminha entre eles.
    float countTimeToBossRun;
    bool isBossRunning;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        animator = GetComponent<Animator>();

        gunScript = GetComponentInChildren<GunsEnemy>();

        ignoreLayermask = LayerMask.GetMask("GunEnemy") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("Default") | LayerMask.GetMask("ShotThrough");

        countTimeToBossRun = timeToBossRun;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, Mathf.Infinity, ~ignoreLayermask);
        // Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!isBossRunning)
                {
                    countTimeToBossRun -= Time.fixedDeltaTime;
                }
                
                if (countTimeToBossRun < 0)
                {
                    RemoveNearestPointFromPlayer();
                }
            }
        }
        SearchAgain();

    }

    private void FixedUpdate()
    {
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    void RemoveNearestPointFromPlayer()
    {
        isBossRunning = true;

        GameObject nearestPointFromPlayer = null; // Ponto mais pr�ximo do Jogador.
        float nearestDistance = Mathf.Infinity; // Dist�ncia do ponto at� o jogador.

        // Verifica a arma mais pr�xima.
        foreach (var point in localPoints)
        {
            point.SetActive(true); // Ativa todos os points desativados.

            float distance = Vector3.Distance(player.transform.position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPointFromPlayer = point; // Seleciona o ponto mais pr�ximo.
            }
        }

        // Desativa o ponto mais perto do Player.
        nearestPointFromPlayer.SetActive(false);

        // Procura o ponto mais perto do Boss para desativa-lo.
        RemoveNearestPointFromBoss();
    }

    void RemoveNearestPointFromBoss()
    {
        GameObject nearestPointFromBoss = null; // Ponto mais pr�ximo do Boss.
        float nearestDistance = Mathf.Infinity; // Dist�ncia do ponto at� o Boss.

        // Verifica a arma mais pr�xima.
        foreach (var point in localPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPointFromBoss = point; // Seleciona o ponto mais pr�ximo.
            }
        }

        // Desativa o ponto mais perto do Boss.
        nearestPointFromBoss.SetActive(false);

        // Procura um novo ponto para o Boss ir.
        SearchNewPoint(); 
    }

    void SearchNewPoint()
    {
        GameObject nearestPointFromBoss = null; // Ponto mais pr�ximo do Boss com exce��o do ponto que estiver perto do Player.
        float nearestDistance = Mathf.Infinity; // Dist�ncia do ponto at� o Boss.

        foreach (var point in localPoints)
        {
            if (point.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, point.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPointFromBoss = point; // Seleciona o ponto mais pr�ximo do Boss, com exce��o do ponto que estiver perto do Player.
                }
            }
            
        }

        agent.SetDestination(nearestPointFromBoss.transform.position);
        animator.SetBool("Walk", true);

        countTimeToBossRun = timeToBossRun;
    }

    void SearchAgain()
    {
        if (agent.pathPending || agent.remainingDistance > 0.5f || agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            return;
        }

        animator.SetBool("Walk", false);
        isBossRunning = false;
    }
}