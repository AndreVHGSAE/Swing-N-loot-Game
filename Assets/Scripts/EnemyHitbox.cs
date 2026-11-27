using UnityEngine;

public class EnemyHitbox : DmgObstacle
{
    public BasicEnemy EnemyBasic;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!EnemyBasic.knockedOut && !EnemyBasic.stunned)
        {
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().Died();
            }
            if (collision.tag == "Graple")
            {
                EnemyBasic.TakeDamage();
                EnemyBasic.rb.AddForceY(10);
                float dir = Mathf.Sign((transform.position - collision.transform.position).x);
                EnemyBasic.rb.AddForceX(dir*10);
            }
        }
    }
}
