using UnityEngine;

public class EnemyVisualTrigger : MonoBehaviour
{
    public BasicEnemy EnemyBasic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !(EnemyBasic.stunned && EnemyBasic.knockedOut))
        {
            if (EnemyBasic != null)
            {
                EnemyBasic.Agro = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !(EnemyBasic.stunned && EnemyBasic.knockedOut))
        {
            if (EnemyBasic != null)
            {
                EnemyBasic.Agro = false;
                EnemyBasic.Flip();
            }
        }
    }
}
