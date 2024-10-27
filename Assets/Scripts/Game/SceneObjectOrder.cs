using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneObjectOrder : MonoBehaviour
{
    GameObject player;
    private Dictionary<GameObject, int> initialSortingOrders = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
    }

    // Update is called once per frame
    

    void Update()
    {
        if (player == null) return; // Verifica se o jogador existe antes de continuar.

        foreach (var obj in GameObject.FindGameObjectsWithTag("SceneObject"))
        {
            SpriteRenderer objSprite = obj.GetComponent<SpriteRenderer>();
            if (objSprite != null)
            {
                // Se não armazenou o sortingOrder inicial, faz isso agora.
                if (!initialSortingOrders.ContainsKey(obj))
                {
                    initialSortingOrders[obj] = objSprite.sortingOrder; // Armazena o sortingOrder inicial.
                }

                int objOrder = initialSortingOrders[obj]; // Recupera o sortingOrder inicial.

                // A ordem do objeto fica em 4 (acima da parede) enquanto o player está dentro dela.
                if (player.transform.position.y > obj.transform.position.y)
                {
                    objSprite.sortingOrder = objOrder + 4;
                }
                else
                {
                    objSprite.sortingOrder = objOrder;
                }
            }
        }

        /*foreach (var wall in GameObject.FindGameObjectsWithTag("WallTransparent"))
        {
            SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
            if (wallSprite != null && player != null)
            {
                // Ajusta a ordem dos inimigos para que fiquem acima ou abaixo da parede dependendo da sua posição
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
                    if (enemySprite != null)
                    {
                        // Verifica a posição do inimigo em relação à parede
                        if (enemy.transform.position.y > wall.transform.position.y && enemy.transform.position.y < wall.transform.position.y + 2.62)
                        {
                            enemySprite.sortingOrder = -1; // Inimigo acima da parede
                        }
                        else
                        {
                            enemySprite.sortingOrder = 4; // Inimigo abaixo da parede
                        }
                    }
                }

                SpriteRenderer playrSprite = player.GetComponent<SpriteRenderer>();
                // Verifica a posição do inimigo em relação à parede
                if (player.transform.position.y > wall.transform.position.y && player.transform.position.y < wall.transform.position.y + 2.62)
                {
                    playrSprite.sortingOrder = -1; // Inimigo acima da parede

                }
                else
                {
                    playrSprite.sortingOrder = 4; // Inimigo abaixo da parede
                }
                foreach (var gunPlayer in GameObject.FindGameObjectsWithTag("GunPlayer"))
                {
                    SpriteRenderer gunPlayerSprite = gunPlayer.GetComponent<SpriteRenderer>();
                    if (gunPlayerSprite != null)
                    {
                        // Verifica a posição do inimigo em relação à parede
                        if (gunPlayer.transform.position.y > wall.transform.position.y && gunPlayer.transform.position.y < wall.transform.position.y + 2.62)
                        {
                            gunPlayerSprite.sortingOrder = -2; // Inimigo acima da parede
                        }
                        else
                        {
                            gunPlayerSprite.sortingOrder = 3; // Inimigo abaixo da parede
                        }
                    }
                }

                foreach (var gunEnemy in GameObject.FindGameObjectsWithTag("GunEnemy"))
                {
                    SpriteRenderer gunEnemySprite = gunEnemy.GetComponent<SpriteRenderer>();
                    if (gunEnemySprite != null)
                    {
                        // Verifica a posição do inimigo em relação à parede
                        if (gunEnemy.transform.position.y > wall.transform.position.y && gunEnemy.transform.position.y < wall.transform.position.y + 2.62)
                        {
                            gunEnemySprite.sortingOrder = -2; // Inimigo acima da parede
                        }
                        else
                        {
                            gunEnemySprite.sortingOrder = 3; // Inimigo abaixo da parede
                        }
                    }
                }
            }
        }*/


    }
}
