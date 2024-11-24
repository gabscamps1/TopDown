using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    GameObject player;
    Color transparenceColor;
    [SerializeField] Vector2 size;
    GameObject[] wallsInArea;
    // bool inArea;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
        transparenceColor = new Color(1, 1, 1, 0.5f); // Cor de transparência da parede.
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        

        GameObject[] allwalls = GameObject.FindGameObjectsWithTag("WallTransparent");
        
        Collider2D[] allObjectsInArea = Physics2D.OverlapBoxAll(player.transform.position, size, 0);

        wallsInArea = allwalls.Where(wall => allObjectsInArea.Any(collider => collider.gameObject == wall)).ToArray();

        PlayerInWallArea();

        //if (inArea == true)
        //{
            foreach (var wall in wallsInArea)
            {
                SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
                if (wallSprite != null)
                {
                    // A parede perde opacidade e sua order fica em 5 (Acima do Player) enquanto o player está dentro dela.
                    if (player.transform.position.y > wall.transform.position.y && player.transform.position.y < wall.transform.position.y + 2)
                    {
                        wallSprite.color = transparenceColor;
                    }
                    else
                    {
                        wallSprite.color = Color.white; // Restaura a cor original (opacidade total)
                    }

                }
            }

            // Para todas as paredes que não estão dentro da área de colisão, restaura a opacidade
            foreach (var wall in allwalls)
            {
                if (!wallsInArea.Contains(wall)) // Verifica se a parede não está na lista das paredes em colisão
                {
                    SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
                    if (wallSprite != null)
                    {
                        wallSprite.color = Color.white; // Restaura a cor original (opacidade total)
                    }
                }
            }
        //}



        void PlayerInWallArea()
        {
            GameObject[] allwalls = GameObject.FindGameObjectsWithTag("WallTransparent");

            foreach (var wall in wallsInArea)
            {
                if (player.GetComponent<Collider2D>().IsTouching(wall.GetComponent<BoxCollider2D>()))
                {
                    // inArea = true;
                    return;
                }
            }

        }



        /*foreach (var wall in GameObject.FindGameObjectsWithTag("WallTransparent"))
        {
            SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
            if (wallSprite != null && player != null)
            {
                // A parede perde opacidade e sua order fica em 5 (Acima do Player) enquanto o player está dentro dela.
                if (player.transform.position.y > wall.transform.position.y && player.transform.position.y < wall.transform.position.y + 2)
                {
                    wallSprite.color = transparenceColor;
                    //wallSprite.sortingOrder = 5;
                }
                else
                {
                    wallSprite.color = Color.white;
                }

                // A order da parede volta para 0 (Abaixo do Player) quando o player está na abaixo dela.
                if (player.transform.position.y < wall.transform.position.y)
                {
                    //wallSprite.sortingOrder = 0;
                }

            }
        }*/
    }
}
