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
    public float rotationSpeed = 20f; // Velocidade da rotação
    public float rotationRange = 15f; // Ângulo máximo de rotação

    private float time;

    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;

        // Obtém o componente Animator anexado ao GameObject
        animator = GetComponent<Animator>();

        // Inicia a animação, assumindo que ela está definida como "Idle" ou "StartAnimation" no Animator Controller
       
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