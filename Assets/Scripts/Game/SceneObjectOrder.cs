using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectOrder : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player; // Pega a referência do Player do GameManager.
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("SceneObject"))
        {
            SpriteRenderer objSprite = obj.GetComponent<SpriteRenderer>();
            if (objSprite != null && player != null)
            {
                int objOrder = objSprite.sortingOrder; // Order inicial do objeto.

                // A order do objeto fica em 4 (Acima da parede) enquanto o player está dentro dela.
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
    }
}
