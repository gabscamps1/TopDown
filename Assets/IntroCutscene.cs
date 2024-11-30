


using System.Collections; // Necessário para corrotinas
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public DialogueTrigger otherScript; // Referência ao outro script

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.2f); // Aguarda 1 segundo
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

