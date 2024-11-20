using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionBarril : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] CircleCollider2D areaExplosion;
    [SerializeField] float damage;
    bool isExploding;
    [SerializeField] AudioClip barrelExplosionSound;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("GunPlayer") || other.CompareTag("GunEnemy"))
        {
            life--;

            if (life <= 0)
            {
                SpriteRenderer barril = GetComponent<SpriteRenderer>();
                barril.enabled = false;

                isExploding = true;

                areaExplosion.enabled = false;

                Destroy(gameObject, 2f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isExploding) Explosion();
    }

    private void Explosion()
    {
        if (SoundFXManager.instance != null && barrelExplosionSound != null)
            SoundFXManager.instance.PlaySoundFXClip(barrelExplosionSound, transform, 1f);

        isExploding = false;

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, areaExplosion.radius * 1.5f);

        HashSet<GameObject> damagedCharacter = new HashSet<GameObject>();

        foreach (var obj in objects)
        {
            GameObject character = obj.gameObject;

            if (character.CompareTag("Player"))
            {
                if (damagedCharacter.Contains(character)) return;

                PlayerDamage playerScript = character.GetComponent<PlayerDamage>();
                playerScript.CallDamage(1);

                damagedCharacter.Add(character);
            }

            if (character.CompareTag("Enemy"))
            {
                if (damagedCharacter.Contains(character)) return;

                EnemyDamage enemyScript = character.GetComponent<EnemyDamage>();
                enemyScript.CallDamage(damage);

                damagedCharacter.Add(character);
            }
        }

    }

    void OnDrawGizmos()
    {
        // Desenha um círculo na Scene View para mostrar a área da explosão
        Gizmos.color = Color.red;  // Cor do círculo
        Gizmos.DrawWireSphere(transform.position, areaExplosion.radius * 1.5f);  // Desenha o círculo com o raio definido
    }

}
