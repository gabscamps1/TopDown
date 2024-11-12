using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ObjectDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    [SerializeField] private float lives;

    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunPlayer acerte o Inimigo a fun��o CallDamage � chamada.
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

            // Se a arma tiver rigidbody e a velocidade (x + y) dela estiver superior a 3, a fun��o CallDamage � chamada.
            float gunVelocity = Mathf.Abs(gunRig.velocity.x) + Mathf.Abs(gunRig.velocity.y);
            if (gunRig != null && gunVelocity > 3)
            {
                GameObject player = GameManager.instance.player;

                // Se o player estiver em cena a fun��o CallDamage � chamada.
                if (player != null)
                {
                    // Pega o Dano que o objeto arremessado causa no Script do GunsPickup que fica no Prefab do Player.
                    CallDamage(player.GetComponentInChildren<GunsPickup>().damage);

                    // Destr�i o Objeto arremessado.
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    // Fun��o que causa dano ao Inimigo.
    void CallDamage(float damage)
    {
        // Diminui a vida do objeto.
        lives -= damage;

        // Destr�i o Enemy quando lives � 0.
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }



}
