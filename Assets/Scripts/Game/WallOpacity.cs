using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    GameObject player;
    Color transparenceColor;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
        transparenceColor = new Color(1, 1, 1, 0.5f); // Cor de transparência da parede.
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var wall in GameObject.FindGameObjectsWithTag("WallTransparent"))
        {
            SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
            if (wallSprite != null && player != null)
            {
                // A parede perde opacidade e sua order fica em 5 (Acima do Player) enquanto o player está dentro dela.
                if (player.transform.position.y > wall.transform.position.y && player.transform.position.y < wall.transform.position.y + 2)
                {
                    wallSprite.color = transparenceColor;
                    wallSprite.sortingOrder = 5;
                }
                else
                {
                    wallSprite.color = Color.white;
                }

                // A order da parede volta para 0 (Abaixo do Player) quando o player está na abaixo dela.
                if (player.transform.position.y < wall.transform.position.y)
                {
                    wallSprite.sortingOrder = 0;
                }
            }
        }
    }
}
