using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody2D rb;
    public Animator anim;

    public int hp = 3;
    public float stunTime = 2;
    public float stunTimeCounter =0;
    public bool stunned = false;
    public bool knockedOut = false;

    [Header("Move info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float idleTime = 2;
    [SerializeField] public float idleTimeCounter;

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
    protected int facingDirection = -1;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        Agro = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("agro", Agro);
        if (!knockedOut && !stunned)
        {
            if (!Agro)
                Patrol();
            else
                ChasePlayer();
        }


        if (stunned)
        {
            anim.SetBool("stunned", stunned);
            stunTimeCounter += Time.deltaTime;
            if (stunTimeCounter >= stunTime)
            {
                stunned = false;
                anim.SetBool("stunned", stunned);
                stunTimeCounter = 0;
            }
        }

        if (hp <= 0)
        {
            if (!knockedOut)
                Player.gameObject.GetComponent<PlayerController>().defeatedEnemies++;
            knockedOut = true;
            anim.SetBool("knockedOut", knockedOut);
        }
    }

    public virtual void Patrol()
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

    public virtual void ChasePlayer()
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

    public virtual void TakeDamage()
    {
        hp--;
        stunned = true;
    }

    public virtual void Flip()
    {
        facingDirection = -facingDirection;

        transform.Rotate(0, 180, 0);
    }

    public virtual void CollisionCheck()
    {
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
    }

}
