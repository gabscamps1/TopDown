using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    public float lives;
    public float maxHealth;

    public HealthBar healthBar;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip enemyDamageSound;

    void Start() {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }
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

                   
                    collider.gameObject.GetComponent<GunsPlayer>().PlayBreakSound(); //Toca o som da arma ou object quebrando

                    // Destrói o Objeto arremessado.
                    Destroy(collider.gameObject);
                }
            }
        }
    }


    // Função que causa dano ao Inimigo.
    public void CallDamage(float damage)
    {
        StartCoroutine(Blink());

        // Diminui a vida do Inimigo.
        if (healthBar != null) {
            healthBar.SetHealth(lives);
        }
        
        lives -= damage;

        // Chama o Som de receber dano.
        if (SoundFXManager.instance != null && enemyDamageSound != null)
            SoundFXManager.instance.PlaySoundFXClip(enemyDamageSound, transform, 1f);

        // Destrói o Enemy quando lives é 0.
        if (lives <= 0)
        {
            if (GetComponent<Dropper>())
            {
                GetComponent<Dropper>().DropChance(); // Chama a função de calculo de drop quando o Objecto é destruido.
                GetComponent<Dropper>().DropMoney(); // Chama a função de dinheiro.
            }

            if (GetComponentInChildren<GunsEnemy>())
            {
                ParticleSystem firePartycle = GetComponentInChildren<GunsEnemy>().GetComponentInChildren<ParticleSystem>();
                // Coloca a particula como filha da Hierarchy para evitar de as partículas geradas serem destruidas.
                firePartycle.transform.SetParent(transform.parent);

                // Deixa a partícula no mesmo tamanho.
                firePartycle.transform.localScale *= 1.5f;

                // Destrói a partícula.
                Destroy(firePartycle.gameObject, 2f);
            }
            
            // Destrói o Inimigo.
            Destroy(gameObject);
        }
    }

    // Pisca o Enemy em vermelho quando o Enemy recebe dano.
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
