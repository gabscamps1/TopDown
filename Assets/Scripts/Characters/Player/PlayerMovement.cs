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
    [SerializeField] Collider2D playerCollider; // Collider do Player.

    [Header("InfoPlayer")]
    public float playerSpeed; // Velocidade do player.
    public float runSpeed; // Multiplicador da velocidade quando está correndo.
    private float runPressed = 1; // Pega o valor do runSpeed e usa no Movement().
    [SerializeField] float jumpTableVelocity; // Velocidade durante o pulo na mesa.
    [SerializeField] float dodgeVelocity; // Velocidade do Dodge.
    private Vector3 movement;
    private float moveHorizontal;
    private float moveVertical;

    [Header("InfoToGuns")]
    public float dotProductRight; // dotProduct para direita em relação ao ponteiro do mouse. O valor do dotProductRight é passado para o script Guns.
    public float dotProductUp; // dotProduct para cima em relação ao ponteiro do mouse.

    bool nextTable;
    Vector2 lookDirection;
    bool isDodging;

    
    private void Update()
    {
        Movement();

        Direction();

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dodge());
        }

        if (!isDodging)
        {
            if (GetComponentInChildren<GunsPickup>().hasGun == true)
            {
                animator.SetLayerWeight(1, 1);
                animator.SetLayerWeight(0, 0);
            }
            else
            {
                animator.SetLayerWeight(0, 1);
                animator.SetLayerWeight(1, 0);
            }
        }

    }
    private void FixedUpdate()
    {
        if (!isDodging)
        {
            rdb.velocity = new Vector2(movement.x, movement.y);
        }
    }

    public void Movement() 
    {

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
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

    // Não apagar função abaixo - Teste futuro

    /*private void OnDrawGizmos()
    {
        // Posição do jogador em mundo
        Vector3 playerPosition = transform.position;

        // Posição do mouse em mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Defina Z como 0 para 2D

        // Direção do mouse
        Vector3 mouseDirection = (mousePosition - playerPosition).normalized;

        // Desenha a linha do jogador até a posição do mouse
        Gizmos.color = Color.red; // Cor para a direção do mouse
        Gizmos.DrawLine(playerPosition, mousePosition);
    }*/

    void JumpTable(Vector2 tableSize)
    {
        if (!nextTable) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rdb.AddRelativeForce(new Vector2(jumpTableVelocity,0) * Time.deltaTime,ForceMode2D.Impulse);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("JumpableTable"))
        {
            nextTable = true;

            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), transform.right);

            BoxCollider2D boxCollider2D = collision.gameObject.GetComponent<BoxCollider2D>();
            if (boxCollider2D != null)
                JumpTable(boxCollider2D.bounds.size);
        }
        else
        {
            nextTable = false;
        }
    }

   IEnumerator Dodge()
    {
        isDodging = true;

        // Confere se o Player está em movimento.
        float directionHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se o moveHorizontal for menor que 0.01 retorna 0, se não retorna +- 1.
        float directionVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se o moveVertical for menor que 0.01 retorna 0, se não retorna +- 1.

        animator.SetLayerWeight(2, 1);
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(0, 0);
        
        animator.SetFloat("WalkHorizontal", Mathf.Abs(directionHorizontal));
        animator.SetFloat("WalkVertical", directionVertical);

        // Confere se o Player está em movimento.
        // float directionHorizontal = (Mathf.Abs(moveHorizontal) < 0.01) ? 0 : (int)Mathf.Sign(moveHorizontal); // Se o moveHorizontal for menor que 0.01 retorna 0, se não retorna +- 1.
        // float directionVertical = (Mathf.Abs(moveVertical) < 0.01) ? 0 : (int)Mathf.Sign(moveVertical); // Se o moveVertical for menor que 0.01 retorna 0, se não retorna +- 1.

        // Retorna a direção do movimento do Player.
        Vector2 dodgeMovement = new Vector2(directionHorizontal, directionVertical);

        // Aplica a força na direção do movimento do Player.
        rdb.AddRelativeForce(dodgeMovement * dodgeVelocity, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);

        isDodging = false;

        animator.SetLayerWeight(2, 0);

        //animator.SetLayerWeight(2, 0);
    }
}

