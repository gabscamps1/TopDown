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
    [SerializeField] private ParticleSystem Explosão;


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("GunPlayer") || other.CompareTag("GunEnemy"))
        {
            life--;

            if (life <= 0)
            {
                /*SpriteRenderer barril = GetComponent<SpriteRenderer>();
                barril.enabled = false;*/

                areaExplosion.enabled = false;

                Particulas();

                isExploding = true;
            }
        }
    }

    private void Particulas()
    {
        ParticleSystem Particle = Instantiate(Explosão, transform.position, Quaternion.identity);
        Particle.Clear();
        Particle.Play(true);
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

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Desenha um círculo na Scene View para mostrar a área da explosão
        Gizmos.color = Color.red;  // Cor do círculo
        Gizmos.DrawWireSphere(transform.position, areaExplosion.radius * 1.5f);  // Desenha o círculo com o raio definido
    }

}
