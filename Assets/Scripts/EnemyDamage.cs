
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    [SerializeField] private float lives;

    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunPlayer acerte o Inimigo a função CallDamage é chamada.
        if (particle.CompareTag("GunPlayer"))
        {
            CallDamage(particle.GetComponentInParent<GunsPlayer>().damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("GunPlayer") && collider.IsTouching(damageCollider))
        {
            Rigidbody2D gunRig = collider.GetComponent<Rigidbody2D>();

            // Se a arma tiver rigidbody e a velocidade (x + y) dela estiver superior a 3, a função CallDamage é chamada.
            float gunVelocity = Mathf.Abs(gunRig.velocity.x) + Mathf.Abs(gunRig.velocity.y);
            if (gunRig != null && gunVelocity > 3)
            {
                GameObject player = GameManager.instance.player;

                // Se o player estiver em cena a função CallDamage é chamada.
                if (player != null)
                {
                    CallDamage(player.GetComponentInChildren<GunsPickup>().damage);
                    Destroy(collider.gameObject);
                }
            }
        }
    }


    // Função que causa dano ao Inimigo.
    void CallDamage(float damage)
    {
        StartCoroutine(Blink());

        lives -= damage;

        if (lives <= 0)
        {

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (!renderer)
            {
                renderer = GetComponentInChildren<SpriteRenderer>();
            }
            renderer.enabled = false;
            Destroy(gameObject);
        }
    }

    IEnumerator Blink()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
        }
        renderer.color = new Color(1, 0, 0);

        yield return new WaitForSeconds(0.1f);

        renderer.color = new Color(1, 1, 1);

        yield return new WaitForSeconds(0.1f);
    }


}
