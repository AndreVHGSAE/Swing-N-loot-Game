using UnityEngine;

public class FlyingEnemy : BasicEnemy
{
    public override void Update()
    {
        base.Update();
        if (knockedOut)
        {
            rb.gravityScale = 1;
        }
    }
    public override void Patrol()
    {

        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter < 0)
        {
            Flip();
            idleTimeCounter = idleTime;
        }
    }

    public override void ChasePlayer()
    {

        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, moveSpeed * Time.deltaTime);
        Vector3 direction = Player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
