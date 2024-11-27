using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAreaWhenPass : MonoBehaviour
{
    [SerializeField] Collider2D closeWhenPassArea;
    Component[] components;

    // Start is called before the first frame update
    void Start()
    {
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
}
