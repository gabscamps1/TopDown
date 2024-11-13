using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    private bool isFogActive = true; // Confere se a Fog est� ativa.
    [SerializeField] BoxCollider2D areaFog; // Tamanho da �rea da Fog. Usado para desativar os Inimigos que estiverem dentro da �rea.
    [SerializeField] BoxCollider2D disableFog; // �rea para desativar a Fog.
    private Collider2D[] objects; // Guarda os Inimigos que estiverem dentro da �rea da Fog.
    [SerializeField] float timerToDesativeFog; // Tempo para a desativa��o da Fog depois do Player passar pela colis�o.
    private float countTimerToDesativeFog;

    void Start()
    {
        // Pega todos os objetos dentro da Fog e coloca no array.
        objects = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

        HideObjects();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Desativa a Fog se o Player entrar na area disableFog.
        if (collision.CompareTag("Player") && collision.IsTouching(disableFog))
        {
            countTimerToDesativeFog = timerToDesativeFog;
            AppearObjects();
            
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Desativa a Fog se o Player entrar na area disableFog.
        if (collision.CompareTag("Player") && !collision.IsTouching(disableFog))
        {
            HideObjects();
        }
        
    }

    void HideObjects()
    {
        
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Enemy") || obj.CompareTag("SceneObject"))
            {
                obj.gameObject.SetActive(false); // Desativa todos os objetos do array com a tag Enemy.
            }
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }

    void AppearObjects()
    {

        // Se a fog estiver desativada, ative todos os objetos do dentro da Fog.
        if (countTimerToDesativeFog <= 0)
        {
            foreach (var obj in objects)
            {
                obj.gameObject.SetActive(true);
            }
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
        else
        {
            countTimerToDesativeFog--;
        }
    }
}
