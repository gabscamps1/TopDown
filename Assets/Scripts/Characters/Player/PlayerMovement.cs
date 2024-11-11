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
    public bool isDodging;
    private bool canDodge = true;
    private bool isJumping;
    private Vector3 jumpDirection;
    private enum PlayerState {Walk, WalkG, Dodge};
    private PlayerState state = PlayerState.Walk;


    private void Start()
    {
        
    }

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

        if (Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene("TesteScene");
        }
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("Tuorial 1");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && canDodge)
        {
            StartCoroutine(Dodge());
        }

    }
    private void FixedUpdate()
    {
        if (!isDodging && !isJumping)
        {
            rdb.velocity = new Vector2(movement.x, movement.y);
        }

        if (isJumping)
        {
            // Move o jogador suavemente até a posição
            rdb.velocity = jumpDirection * playerSpeed;
        }

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
        int moveHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se a tecla D ou A for pressionada, retorna +- 1.
        int moveVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se a tecla W ou S for pressionada, retorna +- 1.

        // Movimento quando o Player anda na Diagonal.
        Vector2 moveDiagonal = new Vector2((Mathf.Sqrt(2) / 2) * moveHorizontal, (Mathf.Sqrt(2) / 2) * moveVertical) * playerSpeed * runPressed;

        // Movimento quando o Player anda em só um Vetor.
        Vector3 moveNormal = new Vector2(moveHorizontal, moveVertical) * playerSpeed * runPressed;

        // Armazena o Movimento do Player.
        movement = (moveHorizontal != 0 && moveVertical != 0) ? moveDiagonal : moveNormal;

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
        }
        else if (dotProductUp < 0.6)
        {
            animator.SetBool("Up", false);
        }

        if (dotProductUp < -0.7)
        {
            animator.SetBool("Down", true);
        }
        else if (dotProductUp > -0.6)
        {
            animator.SetBool("Down", false);
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

        // Vai para Layer Invulnerability.
        gameObject.layer = 9;

        // Confere se o Player está em movimento.
        int directionHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se a tecla D ou A for pressionada, retorna +- 1.
        int directionVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se a tecla W ou S for pressionada, retorna +- 1.

        // Desativa a arma.
        GunsPickup gunsPickupScript = GetComponentInChildren<GunsPickup>();
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null && (directionHorizontal != 0 || directionVertical != 0))
        {
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(false);
            gunsPickupScript.enabled = false;
        }

        // Altera para a layer do Dodge.
        state = PlayerState.Dodge;

        // Seta a animação de Dodge para ser chamada.
        animator.SetInteger("WalkHorizontal", Mathf.Abs(directionHorizontal));
        animator.SetInteger("WalkVertical", directionVertical);

        // Retorna a direção do movimento do Player.
        Vector2 dodgeMovement = new Vector2(directionHorizontal, directionVertical);

        // Aplica a força na direção do movimento do Player.
        rdb.AddRelativeForce(dodgeMovement * dodgeVelocity, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);

        isDodging = false;

        // Desativa a layer do Dodge.
        state = PlayerState.Walk;

        // Ativa a arma.
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null)
        {
            gunsPickupScript.enabled = true;
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(true);
        }

        // volta para Layer Player.
        gameObject.layer = 8;
    }


    void JumpTable()
    {
        Vector3 currentDirection = Vector3.zero;
        currentDirection = (Input.GetKey(KeyCode.D) ? Vector3.right : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.A) ? Vector3.left : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.W) ? Vector3.up : currentDirection);
        currentDirection = (Input.GetKey(KeyCode.S) ? Vector3.down : currentDirection);

        float currentDistance = 0;
        currentDistance = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ? 0.5f : currentDistance);
        currentDistance = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ? 0.4f : currentDistance);

        LayerMask jumpLayerMask = LayerMask.GetMask("SceneObjects") | LayerMask.GetMask("ShotThrough");
        RaycastHit2D jumpHit;
        jumpHit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, currentDirection, currentDistance, jumpLayerMask);
        Debug.DrawLine(transform.position + Vector3.up * 0.5f, jumpHit.point);

        if(jumpHit.collider == null) return;

        if (jumpHit.collider.CompareTag("JumpableTable"))
        {
            canDodge = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {

                // Inicia o Jump.
                isJumping = true;

                BoxCollider2D jumpCollider = jumpHit.collider.GetComponent<BoxCollider2D>();
                Vector2 jumpSize = jumpCollider.bounds.size;

                Vector2 targetPosition = (Vector2)transform.position + jumpSize * currentDirection * 1.2f;
                jumpDirection = (targetPosition - (Vector2)transform.position).normalized;

                StartCoroutine(MoveToPosition(targetPosition));
            }
        }
        else
        {
            canDodge = true;
        }
    }

    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        // Distância de erro para considerar que chegou ao destino.
        while (Vector2.Distance((Vector2)transform.position, targetPosition) > 0.04f) 
        {
            // Vai para Layer Invulnerability.
            gameObject.layer = 9;

            yield return null; // Espera até o próximo frame.
        }

        // Quando o destino é alcançado, para o Jump.
        isJumping = false;

        // Quando o destino é alcançado, volta para Layer Player.
        gameObject.layer = 8;
    }
}

