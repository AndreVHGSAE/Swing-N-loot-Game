using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleTime = 2;
    [SerializeField] private float idleTimeCounter;

    public bool Agro = false;

    [Header("CollisionInfo")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private GameObject PlayerCheck;

    private bool groundDetected;
    private bool isWallDetected;
    private int facingDirection = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        Agro = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Agro)
            Patrol();
        else
            ChasePlayer();
    }

    void Patrol()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        CollisionCheck();

        idleTimeCounter -= Time.deltaTime;

        if (idleTimeCounter < 0)
            rb.linearVelocity = new Vector2(moveSpeed * facingDirection, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, 0);

        if (isWallDetected || !groundDetected)
        {
            Flip();
            idleTimeCounter = idleTime;
        }
    }

    void ChasePlayer()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        CollisionCheck();

        Vector2 playerDirection = (Player.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2((moveSpeed*1.5f) * playerDirection.x, rb.linearVelocity.y);

        if (isWallDetected || !groundDetected)
        {
            Flip();
            idleTimeCounter = idleTime;
            Agro = false;
        }
    }

    public void Flip()
    {
        facingDirection = -facingDirection;

        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
    }

}
