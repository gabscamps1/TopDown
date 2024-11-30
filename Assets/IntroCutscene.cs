using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public DialogueTrigger otherScript; // Refer�ncia ao outro script

    void Start()
    {
        if (otherScript != null)
        {
            otherScript.TriggerDialogue("0"); // Chama o m�todo no outro script
        }
        else
        {
            Debug.LogWarning("OtherScript n�o est� atribu�do!");
        }
    }
}
