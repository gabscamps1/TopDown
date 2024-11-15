using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerDamage : MonoBehaviour
{
<<<<<<< HEAD
    [Header("References")]
    public GameObject deathScreen;

    [Header("InfoDamagePlayer")]
    public float lives;
    [SerializeField] float invulnerabilityTime;

=======
    [SerializeField]
    public float lives;
   
>>>>>>> 0c355c755bc32c287ad0a10c0fa793a4add31eeb
    [Header("Player Sounds")]
    [SerializeField] private AudioClip playerDamageSound;
  
    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunEnemy acerte o Player a fun��o CallDamage � chamada.
        if (particle.CompareTag("GunEnemy"))
        {
            CallDamage(1);
        }
    }

    // Fun��o que causa dano ao Inimigo.
    public void CallDamage(float damage)
    {
        // Inicia a coroutine de piscar o Player em vermelho.
        StartCoroutine(Blink());

        // Chama o Som de receber dano.
        SoundFXManager.instance.PlaySoundFXClip(playerDamageSound, transform, 1f);

        // Torna o Player invulneravel durante determinado tempo.
        StartCoroutine(Invulnerability());

        // Diminui a vida do Player.
        lives -= damage;

        // Destr�i o Player quando lives � 0.
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Pisca o Player em vermelho quando o Player recebe dano.
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
<<<<<<< HEAD

    // Torna o PLayer invulneravel durante determinado tempo.
    IEnumerator Invulnerability()
    {
        gameObject.layer = 9;

        yield return new WaitForSeconds(invulnerabilityTime);

        gameObject.layer = 8;
    }


=======
>>>>>>> 0c355c755bc32c287ad0a10c0fa793a4add31eeb
}
