using System.Collections;
using System.Collections.Generic;
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
    public float playerSpeed = 10; // Velocidade do player.
    public float runSpeed = 2; // Multiplicador da velocidade quando est� correndo.
    private float runPressed = 1; // Pega o valor do runSpeed e usa no Movement().
    private Vector3 movement;

    [Header("InfoToGuns")]
    public float dotProductRight; // dotProduct para direita em rela��o ao ponteiro do mouse. O valor do dotProductRight � passado para o script Guns.
    public float dotProductUp; // dotProduct para cima em rela��o ao ponteiro do mouse.

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
    private void FixedUpdate()
    {
        rdb.velocity = new Vector2(movement.x, movement.y);
    }

    void Movement() 
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

        if (dotProductUp > 0.7) animator.SetBool("Up", true);
        else animator.SetBool("Up", false);

        if (dotProductUp < -0.7) animator.SetBool("Down", true);
        else animator.SetBool("Down", false);

        if (dotProductRight > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);


    }

    // N�o apagar fun��o abaixo - Teste futuro

    /*void Direction()
    {
        Vector3 playerPositionPixels = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = (Input.mousePosition - playerPositionPixels).normalized;

        dotProductRight = Vector3.Dot(Vector3.right, mouseDirection);
        dotProductUp = Vector3.Dot(Vector3.up, mouseDirection);

        if (dotProductUp > 0.7) animator.SetBool("Up", true);
        else animator.SetBool("Up", false);

        if (dotProductUp < -0.7) animator.SetBool("Down", true);
        else animator.SetBool("Down", false);

        if (dotProductRight > 0.7) animator.SetBool("Right", true);
        else animator.SetBool("Right", false);

        if (dotProductRight < -0.7) animator.SetBool("Left", true);
        else animator.SetBool("Left", false);

    }*/

    // N�o apagar fun��o abaixo - Teste futuro

    /*private void OnDrawGizmos()
    {
        // Posi��o do jogador em mundo
        Vector3 playerPosition = transform.position;

        // Posi��o do mouse em mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Defina Z como 0 para 2D

        // Dire��o do mouse
        Vector3 mouseDirection = (mousePosition - playerPosition).normalized;

        // Desenha a linha do jogador at� a posi��o do mouse
        Gizmos.color = Color.red; // Cor para a dire��o do mouse
        Gizmos.DrawLine(playerPosition, mousePosition);


    }*/

}

