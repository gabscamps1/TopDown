using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunsEnemy : MonoBehaviour
{
    [Header("References")]
    public GameObject enemy;
    public ParticleSystem fireParticle; // Partícula do projétil a ser disparado.
    private EnemyDetectPlayer enemyScript;

    [Header("InfoGun")]
    public int currentAmmo; // Munição atual na arma.
    private int maxAmmo; // Quantidade máxima de munição da arma carregada.
    public float timePerBullet = 0f; // Tempo entre cada saída de tiro.
    private float countTimePerBullet;
    public float timeReloadPerBullet; // Tempo para recarregar uma unidade de munição.
    public float bulletSpeed = 10f; // Velocidade do disparo.
    private Quaternion initialRotation; // Rotação inicial da Arma.

    [Header("StateGun")]
    public bool canReload = true;
    public bool isReloading = false;

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

        countTimePerBullet -= Time.deltaTime;
        if (enemyScript.sawPlayer == true && currentAmmo > 0 && !isReloading && countTimePerBullet <= 0)
        {
            Shoot();
        }
        else if (enemyScript.sawPlayer == false)
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
        // Se a arma estiver no Player.
        if (enemyScript && enemyScript.player)
        {
            // Subtrair a posição do Player da posição da Arma.
            Vector3 playerDirection = enemyScript.player.transform.position - transform.position;
            
            // Calcular o ângulo em radianos e depois converter para graus.
            float angle = Mathf.Atan2(playerDirection.x, playerDirection.y) * Mathf.Rad2Deg;

            // Rotacionar a arma em volta do Inimigo dependendo da posição do Player.
            if (enemyScript.dotProductRight > 0)
            {
                if (enemyScript.dotProductRight > 0) transform.rotation = Quaternion.Euler(0, transform.rotation.y, -angle + 80);
            }
            else
            {
                if (enemyScript.dotProductRight < 0) transform.rotation = Quaternion.Euler(180, transform.rotation.y, angle - 100);
            }
        }
    }
    
}
