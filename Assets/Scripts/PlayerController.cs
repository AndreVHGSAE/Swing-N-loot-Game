using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;

    public bool facingRight = true;
    private float movingInput;
    private float JumpForce = 8;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (movingInput > 0 && !facingRight)
            Flip();
        else if (movingInput < 0 && facingRight)
            Flip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movingInput * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        //if (facingRight == true)
        //    facingRight = false;
        //else if (facingRight == false)
        //    facingRight = true;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
    }
}

