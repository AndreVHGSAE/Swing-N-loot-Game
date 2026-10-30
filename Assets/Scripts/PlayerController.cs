using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;

    private float movingInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(movingInput * speed, rb.linearVelocity.y);
    }
}
