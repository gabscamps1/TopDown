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
    public Rigidbody2D rdb; // Refer�ncia do Rigidbody2D.
    public Animator animator; // Refer�ncia do Animator.

    [Header("InfoPlayer")]
    public float playerSpeed; // Velocidade do player.
    public float runSpeed; // Multiplicador da velocidade quando est� correndo.
    private float runPressed = 1; // Pega o valor do runSpeed e usa no Movement().
    private Vector3 movement;
    [SerializeField] float jumpTableVelocity; // Velocidade durante o pulo na mesa.
    [SerializeField] float dodgeVelocity; // Velocidade do Dodge.

    [Header("InfoToGuns")]
    [HideInInspector] public float dotProductRight; // dotProduct para direita em rela��o ao ponteiro do mouse. O valor do dotProductRight � passado para o script Guns.
    [HideInInspector] public float dotProductUp; // dotProduct para cima em rela��o ao ponteiro do mouse.

    [Header("StatePlayer")]
    public bool isDodging;
    [SerializeField] bool canDodge = true;
    private bool isJumping;
    private Vector3 jumpDirection;
    private enum PlayerState {Walk, Dodge, JumpTable};
    private PlayerState state = PlayerState.Walk;


    private void Update()
    {
        if (!DialogueManager.isTalking)
        {
            States();

            Movement();

            TryJumpTable();

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

            if (Input.GetKeyDown(KeyCode.Space) && !isDodging && canDodge && (movement.x != 0 || movement.y != 0))
            {
                StartCoroutine(Dodge());
            }
        }
        else {
            animator.SetBool("Walk", false);
        }

    }
    private void FixedUpdate()
    {
        if (!isDodging && !isJumping)
        {
            rdb.velocity = new Vector2(movement.x, movement.y) * playerSpeed * runPressed;
        }

        if (isJumping)
        {
            // Move o jogador suavemente at� a posi��o
            rdb.velocity = jumpDirection * playerSpeed;
        }

    }

    // Configura o Player de acordo com o State.
    void States()
    {
        switch (state)
        {
            case PlayerState.Walk:

                // Se o Player estiver com a arma na m�o.
                if (GetComponentInChildren<GunsPickup>().hasGun == true)
                {
                    // Altera para layer Armmed.
                    animator.SetLayerWeight(1, 1);
                    animator.SetLayerWeight(0, 0);
                    animator.SetLayerWeight(2, 0);
                    animator.SetLayerWeight(3, 0);
                }
                else
                {
                    // Altera para layer Walk Unarmmed.
                    animator.SetLayerWeight(0, 1);
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 0);
                    animator.SetLayerWeight(3, 0);
                }

                Direction(); // Jogador rotaciona na dire��o do Mouse.
                break;

            case PlayerState.Dodge:

                animator.SetLayerWeight(2, 1);
                animator.SetLayerWeight(0, 0);
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(3, 0);
                break;

            case PlayerState.JumpTable:

                animator.SetLayerWeight(3, 1);
                animator.SetLayerWeight(0, 0);
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
                break;
        }
    }

    public void Movement()
    {
        int moveHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se a tecla D ou A for pressionada, retorna +- 1.
        int moveVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se a tecla W ou S for pressionada, retorna +- 1.

        // Movimento quando o Player anda na Diagonal.
        Vector2 moveDiagonal = new Vector2((Mathf.Sqrt(2) / 2) * moveHorizontal, (Mathf.Sqrt(2) / 2) * moveVertical);

        // Movimento quando o Player anda em s� um Vetor.
        Vector3 moveNormal = new Vector2(moveHorizontal, moveVertical);

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
        // Impede a��es durante o Dodge.
        isDodging = true;

        // Vai para Layer Invulnerability.
        gameObject.layer = 9;

        // Coloca o Player na rota��o inicial.
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Confere se o Player est� em movimento.
        int directionHorizontal = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0); // Se a tecla D ou A for pressionada, retorna +- 1.
        int directionVertical = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0); // Se a tecla W ou S for pressionada, retorna +- 1.

        // Se o Player estiver em movimento durante a coroutine Dodge.
        GunsPickup gunsPickupScript = GetComponentInChildren<GunsPickup>();
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null && (directionHorizontal != 0 || directionVertical != 0))
        {
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(false); // Desativa a arma.
            gunsPickupScript.enabled = false; // Desativa o GunsPickup.
        }

        // Altera para a layer do Dodge.
        state = PlayerState.Dodge;

        // Seta a anima��o de Dodge para ser chamada.
        animator.SetInteger("WalkHorizontal", directionHorizontal);
        animator.SetInteger("WalkVertical", directionVertical);

        // Aplica a for�a na dire��o do movimento do Player.
        rdb.AddRelativeForce(movement * dodgeVelocity, ForceMode2D.Impulse);
        
        while (isDodging)
        {
            yield return new WaitForEndOfFrame();
        }

        // Desativa a layer do Dodge.
        state = PlayerState.Walk;

        // Se tiver arma na m�o do Player.
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null)
        {
            gunsPickupScript.enabled = true; // Ativa o GunsPickup.
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(true); // Ativa a arma.
        }

        // volta para Layer Player.
        gameObject.layer = 8;
    }


    void TryJumpTable()
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
        jumpHit = Physics2D.Raycast(transform.position + Vector3.up * 0.125f, currentDirection, currentDistance, jumpLayerMask);
        Debug.DrawLine(transform.position + Vector3.up * 0.125f, jumpHit.point);

        if (jumpHit.collider == null)
        {
            canDodge = true;
            return;
        }

        if (jumpHit.collider.CompareTag("JumpableTable"))
        {
            canDodge = false;

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                BoxCollider2D jumpCollider = jumpHit.collider.GetComponent<BoxCollider2D>();
                Vector2 jumpSize = jumpCollider.bounds.size;

                Vector2 targetPosition = (Vector2)transform.position + jumpSize * currentDirection * 1.2f;
                jumpDirection = (targetPosition - (Vector2)transform.position).normalized;

                StartCoroutine(JumpTable(targetPosition, currentDirection));
            }
        }
        else
        {
            canDodge = true;
        }
    }

    IEnumerator JumpTable(Vector2 targetPosition, Vector2 currentDirection)
    {
        // Inicia o Jump.
        isJumping = true;

        // Vai para Layer JumpInvulnerability.
        gameObject.layer = 13;

        // Coloca o Player na rota��o inicial.
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Se o Player estiver em movimento durante a coroutine Jump.
        GunsPickup gunsPickupScript = GetComponentInChildren<GunsPickup>();
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null)
        {
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(false); // Desativa a arma.
            gunsPickupScript.enabled = false; // Desativa o GunsPickup.
        }

        // Altera para a layer do Dodge.
        state = PlayerState.JumpTable;

        // O Player fica sobre os Objetos no cen�rio.
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        playerSprite.sortingLayerName = "SceneObjectsTop"; 

        // Dist�ncia de erro para considerar que chegou ao destino.
        while (Vector2.Distance((Vector2)transform.position, targetPosition) > 0.04f) 
        {
            animator.SetInteger("WalkVertical", Mathf.FloorToInt(currentDirection.y));
            animator.SetInteger("WalkHorizontal", Mathf.FloorToInt(currentDirection.x));

            yield return null; // Espera at� o pr�ximo frame.
        }

        animator.SetInteger("WalkVertical", 0);
        animator.SetInteger("WalkHorizontal", 0);

        // Altera para a layer do Dodge.
        state = PlayerState.Walk;

        // Se tiver arma na m�o do Player.
        if (gunsPickupScript.inventory[gunsPickupScript.selectGun] != null)
        {
            gunsPickupScript.enabled = true; // Ativa o GunsPickup.
            gunsPickupScript.inventory[gunsPickupScript.selectGun].SetActive(true); // Ativa a arma.
        }

        // Quando o destino � alcan�ado, volta para Layer Player.
        gameObject.layer = 8;

        // O Player volta a renderiza��o padr�o.
        playerSprite.sortingLayerName = "SceneObjects";

        // Quando o destino � alcan�ado, para o Jump.
        isJumping = false;
    }
}

