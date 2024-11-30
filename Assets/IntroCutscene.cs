using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public DialogueTrigger otherScript; // Referência ao outro script

    void Start()
    {
        if (otherScript != null)
        {
            otherScript.TriggerDialogue("0"); // Chama o método no outro script
        }
        else
        {
            Debug.LogWarning("OtherScript não está atribuído!");
        }
    }
}
