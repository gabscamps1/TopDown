using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GunsPlayer : MonoBehaviour
{
    [Header("References")]
    public GameObject player; // Refer�ncia do Player. Configurado no c�digo GunsPickUp.
    [SerializeField] ParticleSystem fireParticle; // Refer�ncia da part�cula do proj�til a ser disparado.
    private Rigidbody2D rb; // Refer�ncia do Rigbody2D da arma.
    private Collider2D[] areaGun; // Refer�ncia do Collider2D da arma.

    [Header("InfoGun")]
    public int currentAmmo; // Muni��o atual na arma.
    public int maxAmmo; // Quantidade m�xima de muni��o da arma carregada.
    [SerializeField] float timePerBullet; // Tempo entre cada sa�da de tiro.
    private float countTimePerBullet;
    private float timeReloadPerBullet; // Tempo para recarregar uma unidade de muni��o.
    [SerializeField] float bulletSpeed = 10f; // Velocidade do disparo.
    public float damage; // Dano causado no Inimigo. � chamado pelo script Damage.
    public Sprite hudIcon; //�cone da Arma que aparece na HUD.

    [Header("Gun Sound")]
    [SerializeField] private AudioClip gunShootSound;
    


    [Header("StateGun")]
    // public bool canReload = true;
    [SerializeField] bool isReloading = false; // Confere se a arma est� recarregando
    public bool isHold; // Confere se a arma est� sendo segurada pelo Player. Configurado no c�digo GunsPickUp.
    [SerializeField] bool isAutomatic; // Confere se a arma � autom�tica.

    [Header("PreconfigGun")]
    public float angleVariance; // Diferen�a de �ngulo entre armas para corrigir o posicionamento da arma em rela��o ao mouse.

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        areaGun = GetComponents<Collider2D>();

        maxAmmo = currentAmmo; // Seta a muni��o total para o valor da muni��o atual. Isso permite a Arma recarregar somente at� a muni��o que ela come�ou.
        countTimePerBullet = timePerBullet; // Reseta o delay para atirar.
        timeReloadPerBullet = 2f/currentAmmo; // Seta o tempo de carregamento de cada muni��o.

        // Altera a velocidade da part�cula de tiro para o valor da bulletSpeed do Inspetor.
        if (fireParticle != null)
        {
            var mainFireParticle = fireParticle.main;
            mainFireParticle.startSpeed = bulletSpeed;
        }
        
    }

    private void OnEnable()
    {
        isReloading = false; // Seta o reloading como falso, quando o c�digo da arma � ativado.
    }

    void Update()
    {
        // Pegar o item: Mover para a posi��o de segurar do jogador.
        if (!DialogueManager.isTalking) { 
            if (isHold == true)
            {
                rb.isKinematic = true; // Desabilita f�sica da arma enquanto o item � carregado.


                FollowMouseRotation();

                // Intervalo entre os disparos da arma.
                countTimePerBullet -= Time.deltaTime;

                // Confere se a arma � autom�tica.
                if (isAutomatic)
                {
                    // Segurar para atirar.
                    if (Input.GetMouseButton(0) && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0 && fireParticle != null)
                    {
                        Shoot();
                    }
                }
                else
                {
                    // Apertar para atirar.
                    if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0 && fireParticle != null)
                    {
                        Shoot();
                    }
                }

                // Inicia o processo de recarga se a muni��o for zero e n�o estiver reloading.
                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading && fireParticle != null)
                {

                    StartCoroutine(Recarregar());
                }
            }
            else
            {
                // Devolver a colis�o da arma se ela n�o estiver com o Player.
                foreach (var collider in areaGun)
                {
                    collider.enabled = true;
                }
            
            }
        }

    }

    // Chama a fun��o de Tiro.
    void Shoot()
    {
        if (SoundFXManager.instance != null && gunShootSound != null)
            SoundFXManager.instance.PlaySoundFXClip(gunShootSound, transform, 1f); // Toca o som do tiro.

        fireParticle.Emit(20); // Emite o tiro.
        currentAmmo--; // Reduz a muni��o ao disparar.
        countTimePerBullet = timePerBullet; // Reseta o delay para atirar.
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
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, angle + angleVariance);
            }
            else
            {
                transform.rotation = Quaternion.Euler(180, transform.rotation.y, -angle + angleVariance);
            }
        }
    }
}
