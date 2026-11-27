using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Graple : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerController player;
    public float lifeTime = 1;
    public float reachTime = 0.3f;
    public bool reachedTarget = false;
    public bool returning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        reachTime -= Time.deltaTime;
        if (reachTime <= 0 && !reachedTarget)
        {
            if (returning)
            {
                player.canMove = true;
                player.rb.gravityScale = player.gravityScale;
                Destroy(this.gameObject);
            }
            rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            reachTime = 0.3f;
            returning = true;
        }
    }

    public void Init(PlayerController p_PLayer)
    {
        player = p_PLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        reachedTarget=true;
        rb.linearVelocity = Vector3.zero;
        player.Swing(transform.position);
        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
