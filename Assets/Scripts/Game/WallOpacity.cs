using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    GameObject player;
    Collider2D playerCollision;
    bool playerTouching;

    Color transparenceColor;

    [SerializeField] Vector2 size;

    GameObject[] wallsInArea;
    Collider2D[] allObjectsInArea;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
        transparenceColor = new Color(1, 1, 1, 0.5f); // Cor de transparência da parede.

        if (player != null) { 
            foreach (var collision in player.GetComponents<Collider2D>())
            {
                if (!collision.isTrigger)
                {
                    playerCollision = collision;
                }
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        allObjectsInArea = Physics2D.OverlapBoxAll(player.transform.position, size, 0);
        
        foreach (var wallInArea in allObjectsInArea)
        {
            if (!wallInArea.CompareTag("WallTransparent")) continue;
            
            if (!wallInArea.isTrigger) continue;

            if (wallInArea.IsTouching(playerCollision))
            {
                playerTouching = true;
                break;
            }
            else
            {
                playerTouching = false;
            }
        }

        GameObject[] allwalls = GameObject.FindGameObjectsWithTag("WallTransparent");

        if (playerTouching)
        {
            foreach (var wall in allObjectsInArea)
            {
                if (!wall.CompareTag("WallTransparent")) continue;

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
        }
        else
        {
            foreach (var wall in allwalls)
            {
                SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
                if (wallSprite != null)
                {
                    wallSprite.color = Color.white; // Restaura a cor original (opacidade total)
                }
            }
        }

        wallsInArea = allwalls.Where(wall => allObjectsInArea.Any(collider => collider.gameObject == wall)).ToArray();
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
    }

    /*void OnDrawGizmos()
    {
        // Ensure there's a player object reference
        if (player != null)
        {
            // Draw a wireframe box in the scene view
            Gizmos.color = Color.red;  // Set the color of the Gizmo
            Gizmos.DrawWireCube(player.transform.position, size);

            // Now perform the OverlapBoxAll to detect colliders in the area
            allObjectsInArea = Physics2D.OverlapBoxAll(player.transform.position, size, 0);

            // Optionally, visualize the colliders within the area
            Gizmos.color = Color.green;  // Set color for detected objects
            foreach (var collider in allObjectsInArea)
            {
                if (collider != null)
                {
                    // Draw a sphere at the position of each detected object
                    Gizmos.DrawSphere(collider.transform.position, 0.2f);
                }
            }
        }
    }*/
}
