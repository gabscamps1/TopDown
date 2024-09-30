using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rdb;
    public float playerSpeed = 10;
    public float runPressed = 1;
    public float runSpeed = 2;

    public GameObject weapon;

    public bool HasGun = false;

    void Start()
    {
        
    }

    void Update()
    {
       
    }

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

        //Direction();
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

        else
        {

        }
    }

    void ThrowWeapon() {
        weapon.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 10, ForceMode2D.Impulse);


    }

    void Direction()
    {
        if (rdb.velocity.x > 0.001) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.001) transform.rotation = Quaternion.Euler(0, 180, 0);

    }

}

