using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    private bool isNearPlayer = false;
    private bool isHoldingItem = false;
    private Transform playerHoldPosition;
    private GameObject player;
    private Rigidbody2D rb;

    public float throwForce = 10f; // For�a com que o item ser� arremessado.

    public GameObject objectToSpawn; // Prefab do proj�til a ser disparado.
    public Transform FirePoint; // Local de onde os proj�teis s�o disparados.
    public ParticleSystem fireParticle; // Part�cula do proj�til a ser disparado.

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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
        fireParticle.startSpeed = bulletSpeed;
    }

    void Update()
    {
        // Verificar se o jogador est� perto e apertou o bot�o direito do mouse.
        if (isNearPlayer && Input.GetMouseButtonDown(1) && !isHoldingItem)
        {
            // Pegar o item: Mover para a posi��o de segurar do jogador.
            if (playerHoldPosition != null)
            {
                transform.position = playerHoldPosition.position;
                transform.SetParent(playerHoldPosition); // Fazer o item seguir o jogador.
                rb.isKinematic = true; // Desabilita f�sica enquanto o item � carregado.
                isHoldingItem = true;
            }
        }
        else if (isHoldingItem && Input.GetMouseButtonDown(1))
        {
            // Soltar o item: Arremess�-lo na dire��o do mouse.
            transform.SetParent(null); // Remove o item como filho do jogador.
            rb.isKinematic = false; // Reativa a f�sica do item.
            isHoldingItem = false;

            // Calcular a posi��o do mouse no mundo e a dire��o para arremessar.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            // Aplicar for�a na dire��o do mouse
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            player = null;
        }

        if (isHoldingItem) {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reloading && canShoot)
            {
                Shoot();
            }

            FollowMouseRotation();

            // Inicia o processo de recarga se a muni��o for zero e n�o estiver reloading.
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !reloading)
            {
                StartCoroutine(Recarregar());
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Verificar se o objeto que colidiu � o jogador.
        if (collision.CompareTag("Player"))
        {
            isNearPlayer = true;
            // Obter a posi��o de segurar do jogador.
            playerHoldPosition = collision.transform.Find("ItemHoldPosition");
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificar se o jogador saiu de perto do item.
        if (collision.CompareTag("Player"))
        {
            Collider2D playerCollider = collision.GetComponent<CircleCollider2D>();
            Collider2D gunCollider = GetComponent<Collider2D>(); 
            if (!playerCollider.IsTouching(gunCollider))
            {
                isNearPlayer = false;
                playerHoldPosition = null;
                player = null;
            }
        }
    }

    void Shoot()
    {
        /*// Instancia o proj�til no ponto de disparo
        GameObject bullet = Instantiate(objectToSpawn, FirePoint.position, FirePoint.rotation);

        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);*/

        fireParticle.Emit(1);

        currentAmmo--; // Reduz a muni��o ao disparar.
    }

    IEnumerator Recarregar()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime); // Espera o tempo total de recarga.

        // Recarga progressiva
        while (currentAmmo < maxAmmo && reserveAmmo>0)
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
