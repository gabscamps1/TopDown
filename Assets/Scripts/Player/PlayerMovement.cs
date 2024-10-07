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

    [Header("InfoToGuns")]
    public float dotProductRight; // dotProduct para direita em rela��o ao ponteiro do mouse. O valor do dotProductRight � passado para o script Guns.
    public float dotProductUp; // dotProduct para cima em rela��o ao ponteiro do mouse.

    private void FixedUpdate()
    {
        Movement();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            runPressed = runSpeed;
        }
        else {
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

        Direction();
    }

    void Movement() {
        
        if (Input.GetKey(KeyCode.D))
        {
            rdb.AddRelativeForce(Vector2.right * playerSpeed * runPressed);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            rdb.AddRelativeForce(Vector2.left * playerSpeed * runPressed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rdb.AddRelativeForce(Vector2.up * playerSpeed * runPressed);

        }

        else if (Input.GetKey(KeyCode.S))
        {
            rdb.AddRelativeForce(Vector2.down * playerSpeed * runPressed);
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

        if (dotProductRight > 0.7) animator.SetBool("Right", true);
        else animator.SetBool("Right", false);

        if (dotProductRight < -0.7) animator.SetBool("Left", true);
        else animator.SetBool("Left", false);

    }

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

