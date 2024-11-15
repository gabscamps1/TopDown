using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float raycastDistance = 1.2f;
    public LayerMask interactionLayer;
    public Vector2 currentDirection = Vector2.right; // Dire��o inicial, alterada usando dotProduct do PlayerMovement.

    public GameObject interactionIcon;


    void Update()
    {
        // Pega a dire��o que o Player est� olhando;
        PlayerMovement playerMovementScript = GetComponent<PlayerMovement>();
        if (playerMovementScript.dotProductUp > 0.7)
        {
            currentDirection = Vector2.up;
        }
        else if (playerMovementScript.dotProductUp < -0.7)
        {
            currentDirection = Vector2.down;
        }
        else if (playerMovementScript.dotProductUp < 0.6 && playerMovementScript.dotProductUp > -0.6)
        {
            currentDirection = transform.right;
        }

        // Lan�ar o raycast na dire��o atual
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), currentDirection, raycastDistance, interactionLayer);
        Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<DialogueTrigger>() != null)
        {
            DrawInteractionIcon(true);


            if (Input.GetKeyDown(KeyCode.F))
            {
                
                switch(hit.collider.gameObject.name)
                {
                    case "Barwoman":
                        print("ola");
                        break;
                }

                // Chama um m�todo no script Trigger do objeto detectado
                // hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue("0");
            }
        }
        else
        {
            DrawInteractionIcon(false);

        }

    }

    
    public void DrawInteractionIcon(bool isDrawing)
    {
        if (isDrawing) {
            interactionIcon.SetActive(true);
        }
        else
        {
            interactionIcon.SetActive(false);

        }



    }
}
