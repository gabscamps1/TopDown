using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    private bool isNearPlayer = false;
    private bool isHoldingItem = false;
    private Transform playerHoldPosition;
    private Rigidbody2D rb;

    public float throwForce = 10f; // For�a com que o item ser� arremessado
    public float bulletSpeed = 10f; //Velocidade do disparo

    public GameObject objectToSpawn;  // Prefab do proj�til a ser disparado
    public Transform FirePoint;     // Local de onde os proj�teis s�o disparados

    public int maxAmmo = 100;    // Quantidade m�xima de muni��o
    public float reloadTime = 1f;    // Tempo total de recarga
    public float timePerBullet = 0.05f; // Tempo para recarregar uma unidade de muni��o
    public bool canShoot = true;
    public int currentAmmo;
    public bool reloading = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Verificar se o jogador est� perto e apertou o bot�o direito do mouse
        if (isNearPlayer && Input.GetMouseButtonDown(1) && !isHoldingItem)
        {
            // Pegar o item: Mover para a posi��o de segurar do jogador
            if (playerHoldPosition != null)
            {
                transform.position = playerHoldPosition.position;
                transform.SetParent(playerHoldPosition); // Fazer o item seguir o jogador
                rb.isKinematic = true; // Desabilita f�sica enquanto o item � carregado
                isHoldingItem = true;
            }
        }
        else if (isHoldingItem && Input.GetMouseButtonDown(1))
        {
            // Soltar o item: Arremess�-lo na dire��o do mouse
            transform.SetParent(null); // Remove o item como filho do jogador
            rb.isKinematic = false; // Reativa a f�sica do item
            isHoldingItem = false;

            // Calcular a posi��o do mouse no mundo e a dire��o para arremessar
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            // Aplicar for�a na dire��o do mouse
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        if (isHoldingItem) {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reloading && canShoot)
            {
                Shoot();
            }

            FollowMouseRotation();

        }

        // Inicia o processo de recarga se a muni��o for zero e n�o estiver reloading
        if (currentAmmo <= 0 && !reloading)
        {
            //StartCoroutine(Recarregar());
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar se o objeto que colidiu � o jogador
        if (collision.CompareTag("Player"))
        {
            isNearPlayer = true;
            // Obter a posi��o de segurar do jogador
            playerHoldPosition = collision.transform.Find("ItemHoldPosition");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificar se o jogador saiu de perto do item
        if (collision.CompareTag("Player"))
        {
            isNearPlayer = false;
            playerHoldPosition = null;
        }
    }

    void Shoot()
    {
        // Instancia o proj�til no ponto de disparo
        GameObject bullet = Instantiate(objectToSpawn, FirePoint.position, FirePoint.rotation);

        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
        //particleSystem.Emit(1);

        currentAmmo--; // Reduz a muni��o ao disparar
    }

    IEnumerator Recarregar()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime); // Espera o tempo total de recarga

        // Recarga progressiva
        while (currentAmmo < maxAmmo)
        {
            currentAmmo++;
            yield return new WaitForSeconds(timePerBullet);
        }

        reloading = false;
    }

    void FollowMouseRotation()
    {
        // Pegar a posi��o do mouse e converter para o espa�o do mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Subtrair a posi��o do objeto da posi��o do mouse para obter a dire��o
        Vector3 direction = mousePosition - transform.position;

        // Calcular o �ngulo em radianos e depois converter para graus
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar apenas a rota��o ao objeto no eixo Z (mantendo a posi��o intacta)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
