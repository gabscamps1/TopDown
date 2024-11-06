using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomCursor : MonoBehaviour
{
    public Animator animator;

    [Header("Cursor Info")]
    public Transform mCursorVisual;
    public Vector3 mDisplacement;

    [Header("Rotation")]
    public float rotationSpeed = 20f; // Velocidade da rota��o
    public float rotationRange = 15f; // �ngulo m�ximo de rota��o

    private float time;

    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;

        // Obt�m o componente Animator anexado ao GameObject
        animator = GetComponent<Animator>();

        // Inicia a anima��o, assumindo que ela est� definida como "Idle" ou "StartAnimation" no Animator Controller
       
        animator.Play("CursorIdle");
        
    }

    void Update()
    {
        mCursorVisual.position = Input.mousePosition + mDisplacement;

        // Faz o cursor oscilar suavemente da esquerda para a direita
        time += Time.deltaTime * rotationSpeed;
        float angle = Mathf.Sin(time) * rotationRange;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
}