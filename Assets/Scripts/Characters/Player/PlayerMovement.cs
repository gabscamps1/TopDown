using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rdb; // Referência do Rigidbody2D.
    public Animator animator; // Referência do Animator.

    [Header("InfoPlayer")]
    public float playerSpeed; // Velocidade do player.
    public float runSpeed; // Multiplicador da velocidade quando está correndo.
    private float runPressed = 1; // Pega o valor do runSpeed e usa no Movement().
    private Vector3 movement;
    [SerializeField] float jumpTableVelocity; // Velocidade durante o pulo na mesa.
    [SerializeField] float dodgeVelocity; // Velocidade do Dodge.
    private bool nextTable;
    private Vector2 lookDirection;

    [Header("InfoToGuns")]
    [HideInInspector] public float dotProductRight; // dotProduct para direita em relação ao ponteiro do mouse. O valor do dotProductRight é passado para o script Guns.
    [HideInInspector] public float dotProductUp; // dotProduct para cima em relação ao ponteiro do mouse.

    [Header("StatePlayer")]
    private bool isDodging;
    private enum PlayerState {Walk, WalkG, Dodge};
    private PlayerState state = PlayerState.Walk;

    private void Update()
    {
        Movement();

        Direction();

        Layers();

        JumpTable();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            runPressed = runSpeed;
        }
        else
        {
            runPressed = 1;
        }

        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("TesteScene1");
        }
        if (Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene("TesteScene");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            StartCoroutine(Dodge());
        }

    }
    private void FixedUpdate()
    {
        if (!isDodging)
        {
            rdb.velocity = new Vector2(movement.x, movement.y);
        }
<<<<<<< HEAD
=======
        //print(movement);
>>>>>>> 5ae49ed15900ba7075d304a4e4ac8ccc4a368cf8
    }

    // Configura a Layer do Player no animator.
    void Layers()
    {
        switch (state)
        {
            case PlayerState.Walk:
                // Se o Player estiver com a arma na mão.
                if (GetComponentInChildren<GunsPickup>().hasGun == true)
                {
                    // Altera para layer Armmed.
                    animator.SetLayerWeight(1, 1);
                    animator.SetLayerWeight(0, 0);
                    animator.SetLayerWeight(2, 0);
                }
                else
                {
                    // Altera para layer Walk Unarmmed.
                    animator.SetLayerWeight(0, 1);
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 0);
                }
                break;

            case PlayerState.Dodge:
                animator.SetLayerWeight(2, 1);
                animator.SetLayerWeight(0, 0);
                animator.SetLayerWeight(1, 0);
                break;
        }
    }

    public void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector2(moveHorizontal, moveVertical) * playerSpeed * runPressed;

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    void Direction()
    {
        Vector3 playerPositionPixels = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = (Input.mousePosition - playerPositionPixels).normalized;

        dotProductRight = Vector3.Dot(Vector3.right, mouseDirection);
        dotProductUp = Vector3.Dot(Vector3.up, mouseDirection);

        if (dotProductUp > 0.7)
        {
            animator.SetBool("Up", true);
            // lookDirection = Vector2.up;
        }
        else if (dotProductUp < 0.6)
        {
            animator.SetBool("Up", false);
            // lookDirection = Vector2.right;
        }

        if (dotProductUp < -0.7)
        {
            animator.SetBool("Down", true);
            // lookDirection = Vector2.down;
        }
        else if (dotProductUp > -0.6)
        {
            animator.SetBool("Down", false);
            // lookDirection = Vector2.right;
        }

        if (dotProductRight > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    IEnumerator Dodge()
    {
        // Impede ações durante o Dodge.
        isDodging = true;

        // Confere se o Player está em movimento.
        float directionHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se a tecla D ou A for pressionada, retorna +- 1.
        float directionVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se a tecla W ou S for pressionada, retorna +- 1.

        // Altera para a layer do Dodge.
        state = PlayerState.Dodge;

        // Seta a animação de Dodge para ser chamada.
        animator.SetFloat("WalkHorizontal", Mathf.Abs(directionHorizontal));
        animator.SetFloat("WalkVertical", directionVertical);

        // Retorna a direção do movimento do Player.
        Vector2 dodgeMovement = new Vector2(directionHorizontal, directionVertical);

        // Aplica a força na direção do movimento do Player.
        rdb.AddRelativeForce(dodgeMovement * dodgeVelocity, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);

        isDodging = false;

        // Desativa a layer do Dodge.
        state = PlayerState.Walk;

    }


    void JumpTable()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rdb.AddRelativeForce(new Vector2(jumpTableVelocity, 0) * Time.deltaTime, ForceMode2D.Impulse);
        }

        Vector3 currentDirection = Vector3.zero;
        currentDirection = (Input.GetKey(KeyCode.D) ? Vector3.right : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.A) ? Vector3.left : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.W) ? Vector3.up : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.S) ? Vector3.down : currentDirection);

        float currentDistance;
        currentDistance = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ? 3f : 0);
        currentDistance = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ? 3f : 0);

        RaycastHit2D jumpHit;
        jumpHit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity);
        Debug.DrawLine(transform.position, jumpHit.point);

        print(jumpHit.point);
    }
}

