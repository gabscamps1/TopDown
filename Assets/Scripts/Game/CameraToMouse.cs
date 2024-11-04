using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToMouse : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused) 
        //if ((!PauseMenu.isPaused) || (!DialogueManager.isTalking)) 
        if (!DialogueManager.isTalking)
            {
            if (player != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 distance = mousePosition - player.transform.position + (Vector3.up * 0.5f);
                transform.position = new Vector3(player.transform.position.x + (distance.x / 7f), player.transform.position.y + 0.5f + (distance.y / 3.5f), -10);
            }
        }
    }
}
