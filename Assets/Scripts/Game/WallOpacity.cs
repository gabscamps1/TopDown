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
        player = GameManager.instance.player;
        transparenceColor = new Color(1, 1, 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var wall in GameObject.FindGameObjectsWithTag("WallTransparent"))
        {
            SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
            if (wallSprite != null && player != null)
            {
                if (player.transform.position.y > wall.transform.position.y-0.48f)
                {
                    wallSprite.color = transparenceColor;
                    wallSprite.sortingOrder = 2;
                }
                else
                {
                    wallSprite.color = Color.white;
                    wallSprite.sortingOrder = 0;
                }
            }
        }
    }
}
