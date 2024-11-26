using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent; // Referêmcia do NavMeshAgent.
    Animator animator; // Referêmcia do Animator.
    GunsEnemy gunScript;
    LayerMask layermask;
    [SerializeField] float moveSpeed; // Velocidade do Boss
    [SerializeField] float timeToBossRun;
    [SerializeField] GameObject[] localPoints; // Pontos no cenário em que o Boss caminha entre eles.
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

        layermask = LayerMask.GetMask("Player") | LayerMask.GetMask("SceneObjects") | LayerMask.GetMask("Invulnerability"); // Layers para serem detectadas no raycast.

        countTimeToBossRun = timeToBossRun;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), direction, Mathf.Infinity, layermask);
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
                    countTimeToBossRun = timeToBossRun;
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

        GameObject nearestPointFromPlayer = null; // Ponto mais próximo do Jogador.
        float nearestDistance = Mathf.Infinity; // Distância do ponto até o jogador.

        // Verifica a arma mais próxima.
        foreach (var point in localPoints)
        {
            point.SetActive(true); // Ativa todos os points desativados.

            float distance = Vector3.Distance(player.transform.position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPointFromPlayer = point; // Seleciona o ponto mais próximo.
            }
        }

        // Desativa o ponto mais perto do Player.
        nearestPointFromPlayer.SetActive(false);

        // Procura o ponto mais perto do Boss para desativa-lo.
        RemoveNearestPointFromBoss();
    }

    void RemoveNearestPointFromBoss()
    {
        GameObject nearestPointFromBoss = null; // Ponto mais próximo do Boss.
        float nearestDistance = Mathf.Infinity; // Distância do ponto até o Boss.

        // Verifica a arma mais próxima.
        foreach (var point in localPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPointFromBoss = point; // Seleciona o ponto mais próximo.
            }
        }

        // Desativa o ponto mais perto do Boss.
        nearestPointFromBoss.SetActive(false);

        // Procura um novo ponto para o Boss ir.
        SearchNewPoint(); 
    }

    void SearchNewPoint()
    {
        GameObject nearestPointFromBoss = null; // Ponto mais próximo do Boss com exceção do ponto que estiver perto do Player.
        float nearestDistance = Mathf.Infinity; // Distância do ponto até o Boss.

        foreach (var point in localPoints)
        {
            if (point.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, point.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPointFromBoss = point; // Seleciona o ponto mais próximo do Boss, com exceção do ponto que estiver perto do Player.
                }
            }
            
        }
        print(nearestPointFromBoss);
        agent.SetDestination(nearestPointFromBoss.transform.position);
        animator.SetBool("Walk", true);
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
