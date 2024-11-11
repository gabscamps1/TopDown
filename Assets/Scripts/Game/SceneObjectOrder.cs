using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectOrder : MonoBehaviour
{
    GameObject player;
    GameObject[] objects;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
        AdjustSortingOrder();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustEnemySortingOrder();
        AdjustPlayerSortingOrder();
    }

    void AdjustSortingOrder()
    {
        // GameObject[] objects = GameObject.FindGameObjectsWithTag("SceneObject") GameObject.FindGameObjectsWithTag("WallTransparent");

        GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag("SceneObject");
        GameObject[] wallTransparentObjects = GameObject.FindGameObjectsWithTag("WallTransparent");

        // Cria uma lista para armazenar os objetos combinados
        List<GameObject> combinedObjects = new List<GameObject>();

        // Adiciona os objetos do array sceneObjects.
        combinedObjects.AddRange(sceneObjects);

        // Adiciona os objetos do  array wallTransparentObjects.
        combinedObjects.AddRange(wallTransparentObjects);

        // Converter a lista de volta para um array.
        objects = combinedObjects.ToArray();

        System.Array.Sort(objects, (a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        for (int i = 0; i < objects.Length; i++)
        {
            SpriteRenderer spriteRenderer = objects[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = (objects.Length - 1 - i) * 3;
            }
        }
    }

    void AdjustPlayerSortingOrder()
    {
        if (player == null) return; // Retorna se não existir Player no cenário.

        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer == null) return;

        // Obtém a posição y do player
        float playerY = player.transform.position.y;

        int sortingOrder = objects.Length * 3;
        // Determina o sortingOrder do player com base na posição y em relação aos objetos
        foreach (GameObject obj in objects)
        {
            float objY = obj.transform.position.y;

            if (playerY < objY)
            {
                sortingOrder -= 3; // Fica atrás de objetos mais altos
            }
        }

        // Atualiza Sorting Layer do player.
        playerRenderer.sortingOrder = ((objects.Length * 3) - 1) - sortingOrder;

        // Atualiza Sorting Layer dos filhos do Player.
        SpriteRenderer[] childRenderers = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in childRenderers)
        {
            // Confere se não é o renderer do próprio Player.
            if (renderer.gameObject != player)
            {
                renderer.sortingOrder = playerRenderer.sortingOrder - 1;

                // Atualiza Sorting Layer da partícula de tiro da arma.
                ParticleSystem particle = renderer.GetComponentInChildren<ParticleSystem>();
                if (particle != null) particle.GetComponentInChildren<Renderer>().sortingOrder = playerRenderer.sortingOrder - 1;
            }
        }


    }

    void AdjustEnemySortingOrder()
    {
        // Pega todos os Inimigos em cena.
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemys)
        {
            SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
            if (enemyRenderer == null) return;

            // Obtém a posição y do enemy.
            float enemyY = enemy.transform.position.y;

            int sortingOrder = objects.Length * 3;
            // Determina o sortingOrder do player com base na posição y em relação aos objetos
            foreach (GameObject obj in objects)
            {
                float objY = obj.transform.position.y;

                if (enemyY < objY)
                {
                    sortingOrder -= 3; // Fica atrás de objetos mais altos
                }
            }

            // Atualiza Sorting Layer do Inimigo.
            enemyRenderer.sortingOrder = ((objects.Length * 3) - 1) - sortingOrder;

            // Atualiza Sorting Layer dos filhos do Inimigo.
            SpriteRenderer[] childRenderers = enemy.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in childRenderers)
            {
                // Confere se não é o renderer do próprio Inimigo.
                if (renderer.gameObject != enemy)
                {
                    renderer.sortingOrder = enemyRenderer.sortingOrder - 1;

                    // Atualiza Sorting Layer da partícula de tiro da arma.
                    ParticleSystem particle = renderer.GetComponentInChildren<ParticleSystem>();
                    if (particle != null) particle.GetComponentInChildren<Renderer>().sortingOrder = enemyRenderer.sortingOrder - 1;
                }
            }
        }
    }

}



