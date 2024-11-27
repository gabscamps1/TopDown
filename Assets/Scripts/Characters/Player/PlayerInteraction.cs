using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float raycastDistance = 2f;
    public LayerMask interactionLayer;
    public Vector2 currentDirection = Vector2.right; // Direção inicial, alterada usando dotProduct do PlayerMovement.

    public GameObject interactionIcon;

    [SerializeField] public AudioClip dialogueBubbleSound;
    GameData gameData;
    private void Start()
    {
        if (GameManager.instance != null)
        {
            gameData = GameManager.instance.gameData;
        }
    }

    void Update()
    {
        // Pega a direção que o Player está olhando;
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

        // Lançar o raycast na direção atual
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


            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gameData == null) return;
                DialogueTrigger dialogueTrigger = hit.collider.gameObject.GetComponent<DialogueTrigger>();
                
                //SoundFXManager.instance.PlaySoundFXClip(dialogueBubbleSound, transform, 1f);
                switch (hit.collider.gameObject.name)
                {
                    
                    case "Barwoman":
                        ChooseFlag(gameData.BarWomanFlag);

                        if (gameData.BarWomanFlag == 0 && (gameData.TALKED_TO_SELLER == false)) {
                            dialogueTrigger.TriggerDialogue("0");
                            
                        } else if ((gameData.BarWomanFlag == 0) && (gameData.TALKED_TO_SELLER)) {
                            dialogueTrigger.TriggerDialogue("1");

                            gameData.BarWomanFlag = 1;

                        }else if ((gameData.BarWomanFlag == 1) && (gameData.deaths == 1)){
                            dialogueTrigger.TriggerDialogue("2");

                        }else if ((gameData.BarWomanFlag == 1) && (gameData.deaths == 2))
                        {
                            dialogueTrigger.TriggerDialogue("3");
                        }else if ((gameData.BarWomanFlag == 1) && (gameData.deaths >= 3))
                        {
                            dialogueTrigger.TriggerDialogue("4");
                        }


                        //ChooseFlag(gameData.BarWomanFlag);

                        break;

                    case "Vendedor":
                        if (gameData.SellerFlag == 0 && (gameData.TALKED_TO_BOSS == false))
                        {
                            dialogueTrigger.TriggerDialogue("0");

                        }else if (gameData.SellerFlag == 0 && (gameData.TALKED_TO_BOSS))
                        {
                            dialogueTrigger.TriggerDialogue("0");
                            gameData.SellerFlag = 1;

                        }

                        break;

                    case "Puxachefe":
                        if (gameData.BossHandyFlag == 0)
                        {
                            dialogueTrigger.TriggerDialogue("0");
                            gameData.BossHandyFlag = 1;
                            gameData.TALKED_TO_BOSS = true;

                        }
                        else if (gameData.BossHandyFlag == 1)
                        {
                            dialogueTrigger.TriggerDialogue("1");
                            //gameData.SellerFlag = 1;

                        }


                        //ChooseFlag(gameData.BarWomanFlag);

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

    void ChooseFlag(int flag)
    {
        switch (GameManager.instance.gameData.currentLevel)
        {
            case "Level1":
                switch (GameManager.instance.gameData.deaths)
                {
                    case 0:
                        flag = 0;
                        break;
                    case 1:
                        flag = 1;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
                break;
        }
    }
}
