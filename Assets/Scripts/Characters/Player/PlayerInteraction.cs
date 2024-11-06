using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float raycastDistance = 2f;
    public LayerMask interactionLayer;
    public GameObject interactionIcon;

    private Vector2 currentDirection = Vector2.up; // Direção inicial, pode ser mudada pelo movimento do jogador.

    void Update()
    {
        // Lançar o raycast na direção atual
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), currentDirection, raycastDistance, interactionLayer);
        // Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<DialogueTrigger>() != null)
        {
            interactionIcon.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                // Chama um método no script Trigger do objeto detectado
                hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue("0");
            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }

        // Exemplo de troca de direção com base na entrada
        if (Input.GetKeyDown(KeyCode.W)) currentDirection = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) currentDirection = Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) currentDirection = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) currentDirection = Vector2.right;
    }

    // Para visualizar o raycast na Scene
    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentDirection * raycastDistance);*/
    }
}
