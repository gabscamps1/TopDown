
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private float lives;

    [SerializeField]
    ParticleSystem explosion;

    private void OnParticleCollision(GameObject particle)
    {
        // Caso a particula com a Tag BulletPlayer acerte o Inimigo a função CallDamage é chamada.
        if (particle.CompareTag("Gun")) CallDamage(particle.GetComponentInParent<GunsPlayer>().damage);
        
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
