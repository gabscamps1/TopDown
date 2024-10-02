using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Guns : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public ParticleSystem fireParticle; // Partícula do projétil a ser disparado.
    private Rigidbody2D rb;
    private Collider2D areaGun;

    [Header("InfoGun")]
    private int maxAmmo; // Quantidade máxima de munição da arma carregada.
    public int currentAmmo; // Munição atual na arma.
    public float timePerBullet = 0f; // Tempo entre cada saída de tiro.
    private float countTimePerBullet;
    public float timeReloadPerBullet; // Tempo para recarregar uma unidade de munição.
    public float bulletSpeed = 10f; // Velocidade do disparo.

    [Header("StateGun")]
    public bool canReload = true;
    public bool isReloading = false;
    public bool isHold;
    public bool isAutomatic;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        areaGun = GetComponent<Collider2D>();

        maxAmmo = currentAmmo;
        countTimePerBullet = timePerBullet;
        timeReloadPerBullet = 2f/currentAmmo;

        var mainFireParticle = fireParticle.main;
        mainFireParticle.startSpeed = bulletSpeed; // Altera a velocidade da partícula de tiro para o valor da bulletSpeed do Inspetor.
    }

    private void OnEnable()
    {
        isReloading = false; // Seta o reloading como falso, quando o código da arma é ativado.
    }

    void Update()
    {
        // Pegar o item: Mover para a posição de segurar do jogador.
        if (isHold == true)
        {
            rb.isKinematic = true; // Desabilita física enquanto o item é carregado.
            FollowMouseRotation();

            countTimePerBullet -= Time.deltaTime;
            if (isAutomatic)
            {
                if (Input.GetMouseButton(0) && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0)
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0)
                {
                    Shoot();
                }
            }

            // Inicia o processo de recarga se a munição for zero e não estiver reloading.
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
            {
                StartCoroutine(Recarregar());
            }
        }
        else
        {
            areaGun.enabled = true;
        }
            
    }

    // Chama a função de Tiro.
    void Shoot()
    {
        fireParticle.Emit(1);
        currentAmmo--; // Reduz a munição ao disparar.
        countTimePerBullet = timePerBullet;
    }

    // Chama a função de Recarregar.
    IEnumerator Recarregar()
    {
        isReloading = true;
        yield return new WaitForSeconds(timeReloadPerBullet * currentAmmo); // Espera o tempo total de recarga.

        // Recarga progressiva
        while (currentAmmo < maxAmmo)
        {
           currentAmmo++;
           yield return new WaitForSeconds(timeReloadPerBullet);
        }

        isReloading = false;
    }

    void FollowMouseRotation()
    {
        // Pegar a posição do mouse e converter para o espaço do mundo.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Subtrair a posição do objeto da posição do mouse para obter a direção.
        Vector3 direction = mousePosition - transform.position;

        // Calcular o ângulo em radianos e depois converter para graus.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Se a arma estiver no Player.
        if (player)
        {
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

            // Rotacionar a arma em volta do Player dependendo da posição do Mouse.
            if (playerScript.dotProductRight > 0)
            {
                if (playerScript.dotProductRight > 0.3) transform.rotation = Quaternion.Euler(0, transform.rotation.y, angle);
            }
            else
            {
                if (playerScript.dotProductRight < -0.3) transform.rotation = Quaternion.Euler(180, transform.rotation.y, -angle);
            }
        }
    }
}
