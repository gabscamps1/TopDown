using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ObjectDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    [SerializeField] private float lives;
    [SerializeField] public AudioClip objectDestroySound;

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
                    // Pega o Dano que o objeto arremessado causa no Script do GunsPickup que fica no Prefab do Player.
                    CallDamage(player.GetComponentInChildren<GunsPickup>().damage);

                    // Destrói o Objeto arremessado.
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // Função que causa dano ao Inimigo.
    void CallDamage(float damage)
    {
        // Diminui a vida do objeto.
        lives -= damage;

        // Destrói o Enemy quando lives é 0.
        if (lives <= 0)
        {
            if (SoundFXManager.instance != null && objectDestroySound != null)
                SoundFXManager.instance.PlaySoundFXClip(objectDestroySound, transform, 1f);
            Destroy(gameObject);
        }
    }



}
