using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerDamage : MonoBehaviour
{
    [Header("References")]
    public GameObject deathScreen;

    [Header("InfoDamagePlayer")]
    public float lives;
    public bool isDead = false;
    [SerializeField] float invulnerabilityTime;
    bool inInvulnerability;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip playerDamageSound;
    
    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunEnemy acerte o Player a função CallDamage é chamada.
        if (particle.CompareTag("GunEnemy"))
        {
            CallDamage(1);
        }
    }

    // Função que causa dano ao Inimigo.
    public void CallDamage(float damage)
    {
        
        // Retorna o código se o Player estiver invulnerável.
        if (inInvulnerability) return;

        // Torna o Player invulneravel durante determinado tempo.
        StartCoroutine(Invulnerability());

        // Inicia a coroutine de piscar o Player em vermelho.
        StartCoroutine(Blink());

        GameObject.FindObjectOfType<ShakeScreenEffect>().Shake(0.1f, 0.1f);

        // Chama o Som de receber dano.
        if (SoundFXManager.instance != null && playerDamageSound != null)
            SoundFXManager.instance.PlaySoundFXClip(playerDamageSound, transform, 1f);

        // Diminui a vida do Player.
        lives -= damage;

        // Destrói o Player quando lives é 0.
        if (lives <= 0)
        {
            if (GameManager.instance != null)
            {
                isDead = true;
                GameManager.instance.deaths =+ 1; // Pega a quantia de dinheiro que está no GameObject Money e coloca no GameManager.
                GameManager.instance.gameData.died = true;
                
                Destroy(gameObject);
            }
            
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

        // Torna o PLayer invulneravel durante determinado tempo.
    IEnumerator Invulnerability()
    {
        inInvulnerability = true; // Torna o Player invulnerável.

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
        }

        // Duração do efeito de piscar
        float blinkTime = 0f;

        // Continua a piscar enquanto a invulnerabilidade estiver ativa
        while (blinkTime < invulnerabilityTime)
        {
            // Piscando o sprite
            renderer.color = new Color(1, 1, 1, 0.7f); // Torna o sprite semi-transparente
            yield return new WaitForSeconds(0.15f);

            renderer.color = new Color(1, 1, 1, 1); // Restaura a cor original
            yield return new WaitForSeconds(0.15f);

            // Acumulando o tempo de invulnerabilidade.
            blinkTime += 0.3f; // 0.2f para cada mudança de cor, 2 mudanças por ciclo.
        }

        inInvulnerability = false; // Torna o Player vulnerável.
    }
}
