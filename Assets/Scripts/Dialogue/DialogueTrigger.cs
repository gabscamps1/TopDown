using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<DialogosPorFlag> dialogosPorFlag;  // Lista de di�logos organizados por flags
    
    public GameObject interactionIcon;
    public void DrawInteractionIcon(bool isDrawing)
    {
        /*if (isDrawing) {
            interactionIcon.SetActive(true);
        }
        else
        {
            interactionIcon.SetActive(false);

        }*/
        
        

    }

    public void TriggerDialogue(string flag)
    {

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        // Encontrar o di�logo correspondente � flag
        DialogosPorFlag dialogoFlag = dialogosPorFlag.Find(d => d.flag == flag);

        if (dialogoFlag != null)
        {
            dialogueManager.StartDialogue(dialogoFlag.dialogueData);
        }
        else
        {
            Debug.LogWarning("Flag de di�logo n�o encontrada: " + flag);
        }
    }
}
