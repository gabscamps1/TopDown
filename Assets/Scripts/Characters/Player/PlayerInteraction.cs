using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float raycastDistance = 1.2f;
    public LayerMask interactionLayer;
    public Vector2 currentDirection = Vector2.right; // Dire��o inicial, alterada usando dotProduct do PlayerMovement.

    public GameObject interactionIcon;

    [SerializeField] public AudioClip dialogueBubbleSound;


    void Update()
    {
        // Pega a dire��o que o Player est� olhando;
        if (!DialogueManager.isTalking) { 
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
        }

        // Lan�ar o raycast na dire��o atual
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), currentDirection, raycastDistance, interactionLayer);
        Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<DialogueTrigger>() != null)
        {
            

            if (!DialogueManager.isTalking)
            {
                DrawInteractionIcon(true);
            }
            else {
                DrawInteractionIcon(false);
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                //SoundFXManager.instance.PlaySoundFXClip(dialogueBubbleSound, transform, 1f);
                switch (hit.collider.gameObject.name)
                {
                    case "Barwoman":
                        if  (GameManager.instance.gameData.BarWomanFlag == 0){
                            hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue("0");
                            GameManager.instance.gameData.BarWomanFlag = 1;

                        }if (GameManager.instance.gameData.BarWomanFlag == 1)
                        {
                            hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue("1");
                        }


                            break;

                    case "TutorialBoard":
                        //print("ola");
                        hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue("0");
                        break;
                }

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
            //SoundFXManager.instance.PlaySoundFXClip(dialogueBubbleSound, transform, 1f);
        }
        else
        {
            interactionIcon.SetActive(false);

        }



    }
}
