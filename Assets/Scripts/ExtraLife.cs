using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    public int value = 1;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().lives += value;
            Destroy(this.gameObject);
        }
    }
}
