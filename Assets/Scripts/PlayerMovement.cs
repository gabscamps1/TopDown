using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rdb;
    public float playerSpeed = 10;
    public float runPressed = 1;
    public float runSpeed = 2;

    public GameObject weapon;
    public Animator animator;
    public bool HasGun = false;
    public float dotProductRight;
    public float dotProductUp;

    private void FixedUpdate()
    {
        Movement();

        if (Input.GetMouseButtonDown(1) && HasGun)
        {
            //ThrowWeapon();
        }
        

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
        //Horizontal
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

    void ThrowWeapon() 
    {
        weapon.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 10, ForceMode2D.Impulse);
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

