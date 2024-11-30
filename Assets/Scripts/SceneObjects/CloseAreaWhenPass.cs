using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAreaWhenPass : MonoBehaviour
{
    [SerializeField] Collider2D closeWhenPassArea;
    Component[] components;

    GameData gameData;

    public DialogueTrigger otherScript;

    public bool hasActionOccurred = false;

    // Start is called before the first frame update
    void Start() { 

        if (GameManager.instance != null)
        {
            gameData = GameManager.instance.gameData;
        }

        // Pega todos os componentes do GameObject.
        components = GetComponents<Component>();

        // Itera sobre todos os componentes.
        foreach (var component in components)
        {
            // Verifica se o componente não é o próprio script.
            if (component != this && component != closeWhenPassArea)
            {
                // Verifica se o componente é Behaviour.
                if (component is Behaviour)
                {
                    ((Behaviour)component).enabled = false; // Desativa o componente.
                }

                // Verifica se o componente é um SpriteRenderer.
                if (component is SpriteRenderer) 
                {
                    ((SpriteRenderer)component).enabled = false; // Desativa o SpriteRenderer.
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && closeWhenPassArea.IsTouching(collision))
        {
            foreach (var component in components)
            {
                // Verifica se o componente não é o próprio script.
                if (component != this && component != closeWhenPassArea)
                {
                    // Verifica se o componente é Behaviour.
                    if (component is Behaviour)
                    {
                        if (!hasActionOccurred) // Verifica se a ação já aconteceu e se a condição é atendida
                        {
                            StartCoroutine(TriggerCutsceneText());
                            
                            
                            
                            
                        }
                        
                        ((Behaviour)component).enabled = true; // Desativa o componente.
                    }

                    // Verifica se o componente é um SpriteRenderer.
                    if (component is SpriteRenderer)
                    {
                        ((SpriteRenderer)component).enabled = true; // Desativa o SpriteRenderer.
                    }
                }
            }

            closeWhenPassArea.enabled = false;
        }
    }


    IEnumerator TriggerCutsceneText() {
        if (otherScript != null)
        {
            //print("cu");
            hasActionOccurred = true;

            yield return new WaitForSeconds(0.2f); // Aguarda 1 segundo

            if (otherScript != null)
            {
                if (gameData.bossAttempt == 0) //Primeira tentativa
                {
                    otherScript.TriggerDialogue("0");

                }
                else if (gameData.bossAttempt == 1) //Segunda tentativa
                {
                    otherScript.TriggerDialogue("1");

                }
                else if (gameData.bossAttempt == 2) //Terceira tentativa 
                {
                    otherScript.TriggerDialogue("2");

                }
                else if (gameData.bossAttempt >= 3) //Quarta tentativa pra frente
                {
                    otherScript.TriggerDialogue("3");
                }

                
            }
            else
            {
                //Debug.LogWarning("OtherScript não está atribuído!");
            }

            gameData.bossAttempt += 1;
        }

    }
}
