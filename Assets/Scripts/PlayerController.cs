using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    public GameObject bullet;

    public int coins;
    //public List<GameObject> bulletPool = new List<GameObject>();

    [Header("Move Info")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float JumpForce = 8;

    private bool canMove = true;

    [SerializeField] private bool canWallSlide;
    [SerializeField] private bool canWallJump = true;
    [SerializeField] private bool isWallSliding;
    
    public bool facingRight = true;
    [SerializeField] private float movingInput;
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
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //SetBulletPool(bullet, transform.position, bulletPool);
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

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //GameObject temp = GetBullet();
            GameObject temp = Instantiate(bullet, transform.position, transform.rotation);
            temp.SetActive(true);
            temp.transform.position = transform.position;
            temp.transform.rotation = transform.rotation;
            Rigidbody2D rbtemp = temp.GetComponent<Rigidbody2D>();
            rbtemp.AddForce(transform.right * 40, ForceMode2D.Impulse);
        }
    }

    //private void SetBulletPool(GameObject bulletPrefab, Vector3 startPosition, List<GameObject> _bulletPool)
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        GameObject temp = Instantiate(bulletPrefab, startPosition, transform.rotation);
    //        temp.SetActive(false);
    //        _bulletPool.Add(temp);
    //    }
    //}

    //GameObject GetBullet()
    //{
    //    Debug.Log("GettingNormalBullet");

    //    foreach (GameObject b in bulletPool)
    //    {
    //        if (b.activeInHierarchy == false)
    //        {
    //            return b;
    //        }
    //    }
    //    GameObject temp = Instantiate(bullet, transform.position, transform.rotation);
    //    temp.SetActive(false);
    //    return temp;
    //}

    private void Move()
    {
        if (canMove == true)
        rb.linearVelocity = new Vector2(movingInput * speed, rb.linearVelocity.y);
    }

    void FlipController()
    {
        if (isGrounded && isWallDetected)
        {
            if (!facingRight && movingInput > 0)
                Flip();
            else if (facingRight && movingInput < 0)
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
        Vector2 direction = new Vector2(wallJumpDirection.x * -transform.right.x, wallJumpDirection.y);

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

