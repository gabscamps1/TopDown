using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ObjectDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    [SerializeField] private float lives;
    [SerializeField] public AudioClip objectDestroySound;
    [SerializeField] private ParticleSystem Madeira_Quebrada;


    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunPlayer acerte o Inimigo a fun��o CallDamage � chamada.
        if (particle.CompareTag("GunPlayer") || particle.CompareTag("GunEnemy"))
        {
            CallDamage(1);
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
                    CallDamage(1);

                    // Destr�i o Objeto arremessado.
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")|| collision.collider.CompareTag("Enemy"))
        {
            CallDamage(1);
        }
    }

    private void Particulas()
    {
        Instantiate(Madeira_Quebrada, transform.position, Quaternion.identity);
    }

    // Fun��o que causa dano ao Inimigo.
    void CallDamage(float damage)
    {
        // Diminui a vida do objeto.
        lives -= damage;

        // Destr�i o Enemy quando lives � 0.
        if (lives <= 0)
        {
            if (SoundFXManager.instance != null && objectDestroySound != null)
                SoundFXManager.instance.PlaySoundFXClip(objectDestroySound, transform, 1f);

            if (GetComponent<Dropper>())
            {
                GetComponent<Dropper>().DropChance(); // Chama a fun��o de calculo de drop quando o Objecto � destruido.
                GetComponent<Dropper>().DropMoney(); // Chama a fun��o de dinheiro.
            }

            Particulas();
            Destroy(gameObject);
        }
    }



}
