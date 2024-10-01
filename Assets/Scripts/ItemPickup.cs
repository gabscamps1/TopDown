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

    public float throwForce = 10f; // Força com que o item será arremessado.

    public GameObject objectToSpawn; // Prefab do projétil a ser disparado.
    public Transform FirePoint; // Local de onde os projéteis são disparados.
    public ParticleSystem fireParticle; // Partícula do projétil a ser disparado.

    [Header("InfoGun")]
    public int maxAmmo = 100; // Quantidade máxima de munição da arma carregada.
    public int currentAmmo; // Munição atual na arma.
    public int reserveAmmo; // Munição reserva da arma.
    public float reloadTime = 1f;    // Tempo total de recarga.
    public float timePerBullet = 0.05f; // Tempo para recarregar uma unidade de munição.
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
        // Verificar se o jogador está perto e apertou o botão direito do mouse.
        if (isNearPlayer && Input.GetMouseButtonDown(1) && !isHoldingItem)
        {
            // Pegar o item: Mover para a posição de segurar do jogador.
            if (playerHoldPosition != null)
            {
                transform.position = playerHoldPosition.position;
                transform.SetParent(playerHoldPosition); // Fazer o item seguir o jogador.
                rb.isKinematic = true; // Desabilita física enquanto o item é carregado.
                isHoldingItem = true;
            }
        }
        else if (isHoldingItem && Input.GetMouseButtonDown(1))
        {
            // Soltar o item: Arremessá-lo na direção do mouse.
            transform.SetParent(null); // Remove o item como filho do jogador.
            rb.isKinematic = false; // Reativa a física do item.
            isHoldingItem = false;

            // Calcular a posição do mouse no mundo e a direção para arremessar.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            // Aplicar força na direção do mouse
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            player = null;
        }

        if (isHoldingItem) {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reloading && canShoot)
            {
                Shoot();
            }

            FollowMouseRotation();

            // Inicia o processo de recarga se a munição for zero e não estiver reloading.
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !reloading)
            {
                StartCoroutine(Recarregar());
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Verificar se o objeto que colidiu é o jogador.
        if (collision.CompareTag("Player"))
        {
            isNearPlayer = true;
            // Obter a posição de segurar do jogador.
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
        /*// Instancia o projétil no ponto de disparo
        GameObject bullet = Instantiate(objectToSpawn, FirePoint.position, FirePoint.rotation);

        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);*/

        fireParticle.Emit(1);

        currentAmmo--; // Reduz a munição ao disparar.
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
