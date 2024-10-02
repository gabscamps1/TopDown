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
    public int maxAmmo = 100; // Quantidade m�xima de muni��o da arma carregada.
    public int currentAmmo; // Muni��o atual na arma.
    public int reserveAmmo; // Muni��o reserva da arma.
    public float reloadTime = 1f;    // Tempo total de recarga.
    public float timePerBullet = 0.05f; // Tempo para recarregar uma unidade de muni��o.
    public float bulletSpeed = 10f; // Velocidade do disparo.

    [Header("StateGun")]
    public bool canShoot = true;
    public bool reloading = false;
    public bool isHold;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        areaGun = GetComponent<Collider2D>();
        currentAmmo = maxAmmo;
        var mainFireParticle = fireParticle.main;
        mainFireParticle.startSpeed = bulletSpeed; // Altera a velocidade da part�cula de tiro para o valor da bulletSpeed do Inspetor.
    }

    void Update()
    {
        // Pegar o item: Mover para a posi��o de segurar do jogador.
        if (isHold == true)
        {
            rb.isKinematic = true; // Desabilita f�sica enquanto o item � carregado.
            FollowMouseRotation();

            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reloading && canShoot)
            {
                Shoot();
            }

            // Inicia o processo de recarga se a muni��o for zero e n�o estiver reloading.
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !reloading && reserveAmmo != 0)
            {
                StartCoroutine(Recarregar());
            }
        }
        else
        {
            areaGun.enabled = true;
        }
            

    }

    void Shoot()
    {
        fireParticle.Emit(1);
        currentAmmo--; // Reduz a muni��o ao disparar.
    }

    IEnumerator Recarregar()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime); // Espera o tempo total de recarga.

        // Recarga progressiva
        while (currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            currentAmmo++;
            reserveAmmo--;
            yield return new WaitForSeconds(timePerBullet);
        }

        reloading = false;
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
