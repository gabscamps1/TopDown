using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Guns : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public ParticleSystem fireParticle; // Part�cula do proj�til a ser disparado.
    private Rigidbody2D rb;
    private Collider2D areaGun;

    [Header("InfoGun")]
    private int maxAmmo; // Quantidade m�xima de muni��o da arma carregada.
    public int currentAmmo; // Muni��o atual na arma.
    public float timePerBullet = 0f; // Tempo entre cada sa�da de tiro.
    private float countTimePerBullet;
    public float timeReloadPerBullet; // Tempo para recarregar uma unidade de muni��o.
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
        mainFireParticle.startSpeed = bulletSpeed; // Altera a velocidade da part�cula de tiro para o valor da bulletSpeed do Inspetor.
    }

    private void OnEnable()
    {
        isReloading = false; // Seta o reloading como falso, quando o c�digo da arma � ativado.
    }

    void Update()
    {
        // Pegar o item: Mover para a posi��o de segurar do jogador.
        if (isHold == true)
        {
            rb.isKinematic = true; // Desabilita f�sica enquanto o item � carregado.
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

            // Inicia o processo de recarga se a muni��o for zero e n�o estiver reloading.
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

    // Chama a fun��o de Tiro.
    void Shoot()
    {
        fireParticle.Emit(1);
        currentAmmo--; // Reduz a muni��o ao disparar.
        countTimePerBullet = timePerBullet;
    }

    // Chama a fun��o de Recarregar.
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
        // Pegar a posi��o do mouse e converter para o espa�o do mundo.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Subtrair a posi��o do objeto da posi��o do mouse para obter a dire��o.
        Vector3 direction = mousePosition - transform.position;

        // Calcular o �ngulo em radianos e depois converter para graus.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Se a arma estiver no Player.
        if (player)
        {
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

            // Rotacionar a arma em volta do Player dependendo da posi��o do Mouse.
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
