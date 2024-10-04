using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rdb; // Referência do Rigidbody2D.
    public Animator animator; // Referência do Animator.

    [Header("InfoPlayer")]
    public float playerSpeed = 10; // Velocidade do player.
    public float runSpeed = 2; // Multiplicador da velocidade quando está correndo.
    private float runPressed = 1; // Pega o valor do runSpeed e usa no Movement().

    [Header("InfoToGuns")]
    public float dotProductRight; // dotProduct para direita em relação ao ponteiro do mouse. O valor do dotProductRight é passado para o script Guns.
    private float dotProductUp; // dotProduct para cima em relação ao ponteiro do mouse.

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

        if (dotProductUp > 0.5) animator.SetBool("Up",true);
        else animator.SetBool("Up", false);

        if (dotProductUp < -0.5) animator.SetBool("Down", true);
        else animator.SetBool("Down", false);

        if (dotProductRight > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }

}

