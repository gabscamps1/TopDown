using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToMouse : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distance = mousePosition - player.transform.position;
            transform.position = new Vector3(player.transform.position.x + (distance.x / 3.2f), player.transform.position.y + (distance.y / 3), -10);
        }
    }
}
