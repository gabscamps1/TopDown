
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    private float lives;

    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag GunEnemy acerte o Player a função CallDamage é chamada.
        if (particle.CompareTag("GunEnemy"))
        {
            CallDamage(particle.GetComponentInParent<GunsEnemy>().damage);
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
