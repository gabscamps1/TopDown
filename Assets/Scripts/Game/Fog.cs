using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField] BoxCollider2D areaFog; // Tamanho da área da Fog. Usado para desativar os Inimigos que estiverem dentro da área.
    [SerializeField] BoxCollider2D disableFog; // Área para desativar a Fog.
    [SerializeField] BoxCollider2D disableFog1;
    [SerializeField] BoxCollider2D disableFog2;
    private Collider2D[] objects; // Guarda os Inimigos que estiverem dentro da área da Fog.
    [SerializeField] float timerToDesativeFog; // Tempo para a desativação da Fog depois do Player passar pela colisão.
    private float countTimerToDesativeFog;
    bool isFogActive;
    void Start()
    {
        // Pega todos os objetos dentro da Fog e coloca no array.
        objects = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

        HideObjects();
    }

    void Update()
    {
        if (isFogActive) AppearObjects();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Desativa a Fog se o Player entrar na area disableFog.
        if (collision.CompareTag("Player") && (collision.IsTouching(disableFog)|| collision.IsTouching(disableFog1)|| collision.IsTouching(disableFog2)))
        {
            countTimerToDesativeFog = timerToDesativeFog;
            isFogActive = true;
            
        }

    }

    void HideObjects()
    {   
        foreach (var obj in objects)
        {
            if (obj != null)
                if (obj.CompareTag("Enemy") || obj.CompareTag("SceneObject") || obj.CompareTag("GunPlayer"))
                {
                    obj.gameObject.SetActive(false); // Desativa todos os objetos do array com a tag Enemy.
                }
        }
    }

    void AppearObjects()
    {
        // Se a fog estiver desativada, ative todos os objetos do dentro da Fog.
        if (countTimerToDesativeFog <= 0)
        {
            foreach (var obj in objects)
            {
                if(obj != null)
                    if (obj.CompareTag("Enemy") || obj.CompareTag("SceneObject") || obj.CompareTag("GunPlayer"))
                    {
                        obj.gameObject.SetActive(true); // Ativa todos os objetos do array com a tag Enemy.
                    }
            }
            gameObject.SetActive(false);
        }
        else
        {
            countTimerToDesativeFog--;
        }
    }
}
