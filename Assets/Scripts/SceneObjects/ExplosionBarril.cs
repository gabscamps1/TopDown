using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionBarril : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] CircleCollider2D areaExplosion;
    Collider2D areaDamage;
    [SerializeField] float damage;
    bool isExploding;
    bool canExplode;
    [SerializeField] AudioClip barrelExplosionSound;
    LayerMask ignoreLayermask;
    private void Start()
    {
        areaDamage = GetComponent<BoxCollider2D>();

        ignoreLayermask = LayerMask.GetMask("Invulnerability") | LayerMask.GetMask("JumpInvulnerability");
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("GunPlayer") || other.CompareTag("GunEnemy"))
        {
            CallDamage(1);
        }
    }

    public void CallDamage(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            // Impede que o barril cause dano mais de uma vez.
            areaDamage.enabled = false; // Desativa a área de o barril levar dano.
            areaExplosion.enabled = false;
            canExplode = true;

            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Explosion");
            transform.localScale *= 1.5f;
        }
    }

    private void FixedUpdate()
    {
        if (canExplode) StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        canExplode = false;

        

        if (SoundFXManager.instance != null && barrelExplosionSound != null)
            SoundFXManager.instance.PlaySoundFXClip(barrelExplosionSound, transform, 1f);

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, areaExplosion.radius * 1.5f, ~ignoreLayermask);

        HashSet<GameObject> damagedCharacter = new HashSet<GameObject>();

        foreach (var obj in objects)
        {
            GameObject character = obj.gameObject;

            if (damagedCharacter.Contains(character)) continue;

            if (character.CompareTag("Player"))
            {
                PlayerDamage playerScript = character.GetComponent<PlayerDamage>();
                playerScript.CallDamage(1);
            }

            if (character.CompareTag("Enemy"))
            {
                EnemyDamage enemyScript = character.GetComponent<EnemyDamage>();
                enemyScript.CallDamage(damage);
            }

            damagedCharacter.Add(character);
        }

        // Destrói SceneObjects que possuem o script ObjectDamage ou ExplosionBarril.
        foreach (var obj in objects)
        {
            GameObject gameObject = obj.gameObject;

            // Confere se tem a tag SceneObject.
            if (gameObject.CompareTag("SceneObject"))
            {
                // Destrói SceneObjects que possuem o script ObjectDamage.
                ObjectDamage objectScript = gameObject.GetComponent<ObjectDamage>();
                if (objectScript != null)
                    objectScript.CallDamage(damage);

                // Destrói SceneObjects que possuem o script ExplosionBarril.
                if (!obj.isTrigger)
                {
                    ExplosionBarril explosionBarril = gameObject.GetComponent<ExplosionBarril>();
                    if (explosionBarril != null)
                        explosionBarril.CallDamage(1);
                }
            }

        }
        GameObject.FindObjectOfType<ShakeScreenEffect>().Shake(0.4f, 0.1f);
        yield return new WaitForSeconds(0.45f);

        
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Desenha um círculo na Scene View para mostrar a área da explosão
        Gizmos.color = Color.red;  // Cor do círculo
        Gizmos.DrawWireSphere(transform.position, areaExplosion.radius * 1.5f);  // Desenha o círculo com o raio definido
    }

}
