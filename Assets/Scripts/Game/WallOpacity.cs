using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    GameObject player;
    Color transparenceColor;
    [SerializeField] Vector2 size; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
        transparenceColor = new Color(1, 1, 1, 0.5f); // Cor de transparência da parede.
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] wallsInGame = GameObject.FindGameObjectsWithTag("WallTransparent");
        
        Collider2D[] objsCollider = Physics2D.OverlapBoxAll(player.transform.position, size, 0);

        GameObject[] walls = wallsInGame.Where(wall => objsCollider.Any(collider => collider.gameObject == wall)).ToArray();
       
        foreach (var wall in walls)
        {
            SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
            if (wallSprite != null && player != null)
            {
                // A parede perde opacidade e sua order fica em 5 (Acima do Player) enquanto o player está dentro dela.
                if (player.transform.position.y > wall.transform.position.y && player.transform.position.y < wall.transform.position.y + 2)
                {
                    wallSprite.color = transparenceColor;
                }
            }
        }

        // Para todas as paredes que não estão dentro da área de colisão, restaura a opacidade
        foreach (var wall in wallsInGame)
        {
            if (!walls.Contains(wall)) // Verifica se a parede não está na lista das paredes em colisão
            {
                SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
                if (wallSprite != null)
                {
                    wallSprite.color = Color.white; // Restaura a cor original (opacidade total)
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
