using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GunsPlayer : MonoBehaviour
{
    [Header("References")]
    public GameObject player; // Referência do Player. Configurado no código GunsPickUp.
    [SerializeField] ParticleSystem fireParticle; // Referência da partícula do projétil a ser disparado.
    private Rigidbody2D rb; // Referência do Rigbody2D da arma.
    private Collider2D[] areaGun; // Referência do Collider2D da arma.

    [Header("InfoGun")]
    public int currentAmmo; // Munição atual na arma.
    public int maxAmmo; // Quantidade máxima de munição da arma carregada.
    [SerializeField] float timePerBullet; // Tempo entre cada saída de tiro.
    private float countTimePerBullet;
    private float timeReloadPerBullet; // Tempo para recarregar uma unidade de munição.
    [SerializeField] float bulletSpeed = 10f; // Velocidade do disparo.
    public float damage; // Dano causado no Inimigo. É chamado pelo script Damage.
    public Sprite hudIcon; //Ícone da Arma que aparece na HUD.

    [Header("Gun Sound")]
    [SerializeField] private AudioClip gunShootSound;
    


    [Header("StateGun")]
    // public bool canReload = true;
    [SerializeField] bool isReloading = false; // Confere se a arma está recarregando
    public bool isHold; // Confere se a arma está sendo segurada pelo Player. Configurado no código GunsPickUp.
    [SerializeField] bool isAutomatic; // Confere se a arma é automática.

    [Header("PreconfigGun")]
    public float angleVariance; // Diferença de ângulo entre armas para corrigir o posicionamento da arma em relação ao mouse.

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        areaGun = GetComponents<Collider2D>();

        maxAmmo = currentAmmo; // Seta a munição total para o valor da munição atual. Isso permite a Arma recarregar somente até a munição que ela começou.
        countTimePerBullet = timePerBullet; // Reseta o delay para atirar.
        timeReloadPerBullet = 2f/currentAmmo; // Seta o tempo de carregamento de cada munição.

        // Altera a velocidade da partícula de tiro para o valor da bulletSpeed do Inspetor.
        if (fireParticle != null)
        {
            var mainFireParticle = fireParticle.main;
            mainFireParticle.startSpeed = bulletSpeed;
        }
        
    }

    private void OnEnable()
    {
        isReloading = false; // Seta o reloading como falso, quando o código da arma é ativado.
    }

    void Update()
    {
        // Pegar o item: Mover para a posição de segurar do jogador.
        if (!DialogueManager.isTalking) { 
            if (isHold == true)
            {
                rb.isKinematic = true; // Desabilita física da arma enquanto o item é carregado.


                FollowMouseRotation();

                // Intervalo entre os disparos da arma.
                countTimePerBullet -= Time.deltaTime;

                // Confere se a arma é automática.
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

                // Inicia o processo de recarga se a munição for zero e não estiver reloading.
                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading && fireParticle != null)
                {

                    StartCoroutine(Recarregar());
                }
            }
            else
            {
                // Devolver a colisão da arma se ela não estiver com o Player.
                foreach (var collider in areaGun)
                {
                    collider.enabled = true;
                }
            
            }
        }

    }

    // Chama a função de Tiro.
    void Shoot()
    {
        if (SoundFXManager.instance != null && gunShootSound != null)
            SoundFXManager.instance.PlaySoundFXClip(gunShootSound, transform, 1f); // Toca o som do tiro.

        fireParticle.Emit(20); // Emite o tiro.
        currentAmmo--; // Reduz a munição ao disparar.
        countTimePerBullet = timePerBullet; // Reseta o delay para atirar.
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
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, angle + angleVariance);
            }
            else
            {
                transform.rotation = Quaternion.Euler(180, transform.rotation.y, -angle + angleVariance);
            }
        }
    }
}
