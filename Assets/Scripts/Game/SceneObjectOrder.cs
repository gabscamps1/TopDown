using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectOrder : MonoBehaviour
{
    GameObject player;
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
        GameObject[] objects = GameObject.FindGameObjectsWithTag("WallTransparent");
        System.Array.Sort(objects, (a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        for (int i = 0; i < objects.Length; i++)
        {
            SpriteRenderer spriteRenderer = objects[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = (objects.Length - 1 - i) * 2;
            }
        }
    }

    void AdjustPlayerSortingOrder()
    {
        if (player == null) return;

        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            // Obtém a posição y do player
            float playerY = player.transform.position.y;

            // Determina o sortingOrder do player com base na posição y em relação aos objetos
            GameObject[] objects = GameObject.FindGameObjectsWithTag("WallTransparent");
            int sortingOrder = objects.Length * 2;

            foreach (GameObject obj in objects)
            {
                float objY = obj.transform.position.y;

                if (playerY < objY)
                {
                    sortingOrder -= 2; // Fica atrás de objetos mais altos
                }
            }

            // Define o sortingOrder do player.
            playerRenderer.sortingOrder = ((objects.Length * 2) - 1) - sortingOrder;

            SpriteRenderer[] childRenderers = player.GetComponentsInChildren<SpriteRenderer>();

            // Itera sobre cada SpriteRenderer.
            foreach (SpriteRenderer renderer in childRenderers)
            {
                // Muda a render dos filhos para a mesma do Player.
                renderer.sortingOrder = playerRenderer.sortingOrder;

            }
        }

    }

    void AdjustEnemySortingOrder()
    {

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemys)
        {
            SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
            if (enemyRenderer != null)
            {
                // Obtém a posição y do enemy.
                float enemyY = enemy.transform.position.y;

                // Determina o sortingOrder do player com base na posição y em relação aos objetos
                GameObject[] objects = GameObject.FindGameObjectsWithTag("WallTransparent");
                int sortingOrder = objects.Length * 2;

                foreach (GameObject obj in objects)
                {
                    float objY = obj.transform.position.y;

                    if (enemyY < objY)
                    {
                        sortingOrder -= 2; // Fica atrás de objetos mais altos
                    }
                }

                // Define o sortingOrder do player.
                enemyRenderer.sortingOrder = ((objects.Length * 2) - 1) - sortingOrder;

                SpriteRenderer[] childRenderers = player.GetComponentsInChildren<SpriteRenderer>();

                // Itera sobre cada SpriteRenderer.
                foreach (SpriteRenderer renderer in childRenderers)
                {
                    // Muda a render dos filhos para a mesma do Enemy.
                    renderer.sortingOrder = enemyRenderer.sortingOrder;

                }
            }

        }
    }

}



