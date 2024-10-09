using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunsEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ParticleSystem fireParticle; // Partícula do projétil a ser disparado.
    private EnemyDetectPlayer enemyScript;

    [Header("InfoGun")]
    [SerializeField] int currentAmmo; // Munição atual na arma.
    private int maxAmmo; // Quantidade máxima de munição da arma carregada.
    [SerializeField] float timePerBullet = 0f; // Tempo entre cada saída de tiro.
    private float countTimePerBullet;
    private float timeReloadPerBullet; // Tempo para recarregar uma unidade de munição.
    [SerializeField] float bulletSpeed = 10f; // Velocidade do disparo.
    public float damage; // Dano causado no Player. É chamado pelo script Damage.

    [Header("StateGun")]
    [SerializeField] bool isReloading = false;
    private bool sawPlayer;

    [Header("PreconfigGun")]
    [SerializeField] float angleVariance; // Diferença de ângulo entre armas para corrigir o posicionamento da arma em relação ao Player.
    [SerializeField] Vector2 raycastInitialPosition; // Ajuste para a posição inicial do Raycast detect em relação do pivo da Arma.
    private Quaternion initialRotation; // Rotação inicial da Arma.
    

    void Start()
    {
        maxAmmo = currentAmmo;
        countTimePerBullet = timePerBullet;
        timeReloadPerBullet = 2f / currentAmmo;

        var mainFireParticle = fireParticle.main;
        mainFireParticle.startSpeed = bulletSpeed; // Altera a velocidade da partícula de tiro para o valor da bulletSpeed do Inspetor.

        // Pegar o código do Inimigo parent da Arma.
        enemyScript = GetComponentInParent<EnemyDetectPlayer>();
    }

    private void OnEnable()
    {
        isReloading = false; // Seta o reloading como falso, quando o código da arma é ativado.
    }

    void Update()
    {
        FollowMouseRotation();
        DetectPlayer(); // Identifica quando o Player entra na visão da Arma.

        countTimePerBullet -= Time.deltaTime;
        if (sawPlayer == true && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0)
        {
            Shoot();
        }
        
        if (enemyScript.player == null)
        {
            transform.rotation = initialRotation;
        }

        // Inicia o processo de recarga se a munição for zero e não estiver reloading.
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Recarregar());
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
        // Se o Player estiver na área do Inimigo.
        if (enemyScript.player != null)
        {
            // Subtrair a posição do Player da posição da Arma.
            Vector3 playerDirection = (enemyScript.player.transform.position - transform.position).normalized;
            
            // Calcular o ângulo em radianos e depois converter para graus.
            float angle = Mathf.Atan2(playerDirection.x, playerDirection.y) * Mathf.Rad2Deg;

            // Rotacionar a arma em volta do Inimigo dependendo da posição do Player.
            if (enemyScript.dotProductRight > 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, -angle + 80 + angleVariance);
            }
            else
            {
                transform.rotation = Quaternion.Euler(180, transform.rotation.y, angle - 100 + angleVariance);
            }
        }
    }

    private void DetectPlayer()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast")  | LayerMask.GetMask("PlayerChildren"); // Layers para não serem detectadas no raycast.
        RaycastHit2D detect;
        detect = Physics2D.Raycast(transform.position + (transform.right * raycastInitialPosition.x) + (transform.up * raycastInitialPosition.y), transform.right, Mathf.Infinity, ~ignoreLayermask);
        Debug.DrawLine(transform.position + (transform.right * raycastInitialPosition.x) + (transform.up * raycastInitialPosition.y), detect.point);

        if (detect && detect.collider.CompareTag("Player"))
        {
            sawPlayer = true;
        }
        else
        {
            sawPlayer = false;
        }

    }

}
