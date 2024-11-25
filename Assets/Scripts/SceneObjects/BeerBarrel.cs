using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBarrel : MonoBehaviour
{
    
    [SerializeField] Collider2D areaActive;
    [SerializeField] AudioClip barrelExplosionSound;
    Animator animator;
    bool playerInDamageArea;
    GameObject player;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && areaActive.IsTouching(collision))
        {
            StartCoroutine(DelayToAttack());

            player = collision.gameObject;

            playerInDamageArea = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !areaActive.IsTouching(collision))
        {
            playerInDamageArea = false;
        }
    }

    IEnumerator DelayToAttack()
    {
        // Espera 1 segundo para iniciar a anima��o de ataque.
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Attack");
    }

    private void Attack()
    {
        if (SoundFXManager.instance != null && barrelExplosionSound != null)
            SoundFXManager.instance.PlaySoundFXClip(barrelExplosionSound, transform, 1f);

        if (playerInDamageArea)
        {
            player.GetComponent<PlayerDamage>().CallDamage(1);
        }
        areaActive.enabled = true;

        animator.ResetTrigger("Attack");
    }

    /*void OnDrawGizmos()
    {
        // Desenha um ret�ngulo (wireframe) na Scene View para mostrar a �rea da explos�o
        Gizmos.color = Color.red;  // Cor do ret�ngulo
        Vector2 center = (Vector2)transform.position + areaActive.offset * 1.5f;  // Posi��o central do ret�ngulo
        Vector3 size = new Vector3(areaActive.bounds.size.x, areaActive.bounds.size.y, 1);  // Tamanho do ret�ngulo, considerando a escala de X e Y

        Gizmos.DrawWireCube(center, size);  // Desenha o ret�ngulo com a posi��o e o tamanho especificados
    }*/
}
