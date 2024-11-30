


using System.Collections; // Necess�rio para corrotinas
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public DialogueTrigger otherScript; // Refer�ncia ao outro script

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.2f); // Aguarda 1 segundo
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

