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
    public GameObject player; // Referência do Player. Recebe a referência quando o Player entra no collider do Inimigo.
    
    [Header("InfoEnemy")]

    [HideInInspector] public float dotProductRight; // Usado para rotação do Inimigo e rotação da arma do Inimigo no script GunsEnemy.
    Quaternion initialRotation; // Rotação inicial do Inimigo.

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
        }
    }

    void Direction()
    {
        Vector3 playerDirection = (player.transform.position - transform.parent.position).normalized;
        dotProductRight = Vector3.Dot(Vector3.right, playerDirection);
        float dotProductUp = Vector3.Dot(Vector3.up, playerDirection);

        if (dotProductUp > 0.5) animator.SetBool("Up", true);
        else animator.SetBool("Up", false);

        if (dotProductUp < -0.5) animator.SetBool("Down", true);
        else animator.SetBool("Down", false);

        if (dotProductRight > 0) transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        else transform.parent.rotation = Quaternion.Euler(0, 180, 0);
    }

}