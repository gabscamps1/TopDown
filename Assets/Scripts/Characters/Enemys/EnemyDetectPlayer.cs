using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class EnemyDetectPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator; // Referência do animator do Inimigo.
    [SerializeField] SpriteRenderer spriteRenderer; // Referência do SpriteRenderer do Inimigo.
    public GameObject player; // Referência do Player. Recebe a referência quando o Player entra no collider do Inimigo.
    
    [Header("InfoEnemy")]
    public float dotProductRight;
    Quaternion initialRotation; // Rotação inicial do Inimigo.

    [Header("StateEnemy")]
    public bool sawPlayer; // Permite o Inimigo rotacionar em direção ao PLayer.

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (player != null)
        {
            Direction();
        }
        else
        {
            transform.rotation = initialRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
            sawPlayer = false;
        }
    }

    /*private void DetectPlayer()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        LayerMask ignoreLayermask = LayerMask.GetMask("Gun") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ignore Raycast"); // Layers para não serem detectadas no raycast.
        RaycastHit2D detect;
        detect = Physics2D.Raycast(transform.position + transform.right * 0.2f, transform.right, Mathf.Infinity, ~ignoreLayermask);
        Debug.DrawLine(transform.position + transform.right * 0.2f, detect.point);

        if (detect && detect.collider.CompareTag("Player"))
        {
            sawPlayer = true;
        }
        else
        {
            sawPlayer = false;
        }

    }*/

    void Direction()
    {
        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        dotProductRight = Vector3.Dot(Vector3.right, playerDirection);
        float dotProductUp = Vector3.Dot(Vector3.up, playerDirection);

        if (dotProductUp > 0.5) animator.SetBool("Up", true);
        else animator.SetBool("Up", false);

        if (dotProductUp < -0.5) animator.SetBool("Down", true);
        else animator.SetBool("Down", false);

        if (dotProductRight > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }

}