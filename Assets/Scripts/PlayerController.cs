using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public UIScript UIScript;

    public GameObject graple;
    public Transform grapleSpawnPoint;

    public float respawnPointX;
    public float respawnPointY;

    public int coins;
    public int lives;
    public int energy = 100;
    public float gravityScale;
    //public List<GameObject> bulletPool = new List<GameObject>();

    [Header("Move Info")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float swingSpeed = 16;
    [SerializeField] private float JumpForce = 8;

    [SerializeField] public bool canMove = true;
    [SerializeField] private float MoveLockTime = 0.05f;
    private Coroutine LockRoutine;
    [SerializeField] private float DyingTime = 3;
    private bool dying = false;

    [SerializeField] private bool canWallSlide;
    [SerializeField] private bool canWallJump = true;
    [SerializeField] private bool isWallSliding;

    public bool facingRight = true;
    [SerializeField] private float movingInput;
    public int facingDirection = 1;
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

        UIScript.Lives(lives);

        respawnPointX = transform.position.x;
        respawnPointY = transform.position.x;

        gravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        FlipController();
        CollisionCheck();
        AnimatorController();
        UIScript.Lives(lives);
    }

    private void AnimatorController()
    {
        bool isMoving = movingInput != 0;

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
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
        {
            movingInput = Input.GetAxisRaw("Horizontal");
            if (!isWallSliding)
                Shoot();
        }

    }

    public IEnumerator MoveLock(float waitTime)
    {
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
        if (rb.gravityScale <= 0)
        {
            rb.gravityScale = gravityScale;
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

        rb.linearVelocity = Vector2.zero;

        if (LockRoutine != null)
        {
            StopCoroutine(LockRoutine);
        }

        LockRoutine = StartCoroutine(MoveLock(MoveLockTime));

        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && energy > 0)
        {
            GameObject temp = Instantiate(graple, grapleSpawnPoint.position, transform.rotation);
            temp.SetActive(true);
            temp.transform.position = grapleSpawnPoint.position;
            temp.transform.rotation = transform.rotation;
            Rigidbody2D rbtemp = temp.GetComponent<Rigidbody2D>();
            rbtemp.AddForce(transform.right * 30, ForceMode2D.Impulse);
            temp.GetComponent<Graple>().Init(this);
            energy--;
            canMove = false;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Swing(Vector3 swingPoint)
    {
        Vector2 swingDirection = (swingPoint - transform.position);
        if (LockRoutine != null)
        {
            StopCoroutine(LockRoutine);
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(swingDirection.normalized * swingSpeed, ForceMode2D.Impulse);
        float timeToPoint = swingDirection.magnitude/(0+swingSpeed);
        LockRoutine = StartCoroutine(MoveLock(timeToPoint));
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

    public void SetRespawn()
    {
        respawnPointX = transform.position.x;
        respawnPointY = transform.position.y;
    }

    public void Died()
    {
        if (!dying)
        {
            lives--;
            UIScript.Lives(lives);
            StartCoroutine(DyingRoutine());
        }
    }

    public IEnumerator DyingRoutine()
    {
        canMove = false;
        dying = true;
        anim.SetBool("dying", dying);

        rb.linearVelocity = Vector2.zero;
        rb.AddForceY(10);
        float dir = facingDirection;
        rb.AddForceX(dir * 100);

        yield return new WaitForSeconds(DyingTime);

        CheckIfGameOver();
        dying = false;
        anim.SetBool("dying", dying);
        canMove = true;
        transform.position = new Vector3(respawnPointX, respawnPointY, 0);
    }

    public void CheckIfGameOver()
    {
        if (lives <= 0)
        {
            //Time.timeScale = 0;
            SceneManager.LoadScene(0);
        }
    }
}

