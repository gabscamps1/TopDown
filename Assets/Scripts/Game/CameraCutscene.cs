using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCutscene : MonoBehaviour
{
    GameObject player;

    [SerializeField] float stoppedTime; // Tempo que a câmera fica parado no ponto inicial.
    [SerializeField] float transitionTime; // Tempo que a câmera leva para sair do ponto inical e chegar no ponto final.
    [SerializeField] float finalTime; // Tempo que a câmera leva para sair do sair do modo Cutscene e ir para CameraToMouse.
    bool inTransition;
    Vector3 moveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<CameraToMouse>() != null)
            GetComponent<CameraToMouse>().enabled = false;

        if (GameManager.instance != null)
            player = GameManager.instance.player;

        if (player == null) return;

        Vector2 distance = (Vector2)player.transform.position - (Vector2)transform.position;
        moveVelocity = distance / transitionTime;

        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (inTransition)
        {
            transform.position += moveVelocity * Time.deltaTime;
        }
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(stoppedTime);

        inTransition = true;

        yield return new WaitForSeconds(transitionTime);

        inTransition = false;

        yield return new WaitForSeconds(finalTime);

        if (GetComponent<CameraToMouse>())
        {
            GetComponent<CameraToMouse>().enabled = true;
        }

        enabled = false;
    }
}
