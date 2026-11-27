using UnityEngine;

public class EnergyRegain : MonoBehaviour
{
    public int value = 1;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().energy += value;
            Destroy(this.gameObject);
        }
    }
}
