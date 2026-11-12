using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Move Info")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float JumpForce = 8;

    private bool canMove = true;

    [SerializeField] private bool canWallSlide;
    [SerializeField] private bool canWallJump = true;
    [SerializeField] private bool isWallSliding;
    
    public bool facingRight = true;
    private float movingInput;
    public int facingDirection = 1 ;
    [SerializeField] private Vector2 wallJumpDirection;

    [Header("CollisionInfo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private bool isWallDetected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        FlipController();
        CollisionCheck();
        AnimatorController();
    }

    private void AnimatorController()
    {
        bool isMoving = movingInput != 0;

        anim.SetFloat("yVelocity",rb.linearVelocity.y);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsWallSliding", isWallSliding);
    }

    void FixedUpdate()
    {

        if (isWallDetected && canWallSlide && !Input.GetKey(KeyCode.Space))
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(movingInput * speed, rb.linearVelocity.y * 0.2f);
        }
        else
        {
            isWallSliding = false;
            Move();
        }

        if (Input.GetAxis("Vertical") < 0)
            canWallSlide = false;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isWallSliding && canWallJump)
        {
            WallJump();
        }

        if (canMove == true)
            movingInput = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        if (canMove == true)
        rb.linearVelocity = new Vector2(movingInput * speed, rb.linearVelocity.y);
    }

    void FlipController()
    {
        if (isGrounded && isWallDetected)
        {
            if (facingRight && movingInput < 0)
                Flip();
            else if (!facingRight && movingInput < 0)
                Flip();
        }

        if (movingInput > 0 && facingRight)
            Flip();
        else if (movingInput < 0 && !facingRight)
            Flip();
    }

    void Flip()
    {
        facingDirection = -facingDirection;
        facingRight = !facingRight;
        wallCheckDistance = -wallCheckDistance;

        transform.Rotate(0, 180, 0);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
    }

    private void WallJump()
    {
        Vector2 direction = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

        rb.AddForce(direction, ForceMode2D.Impulse);
    }
    private void CollisionCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        if (!isGrounded)
            canWallSlide = true;
        else
            canWallSlide = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance,
                                                        wallCheck.position.y,
                                                        wallCheck.position.z));
    }
}

