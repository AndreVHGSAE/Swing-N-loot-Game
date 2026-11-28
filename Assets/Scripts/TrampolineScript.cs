using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    public int BounceValue;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Jump(BounceValue);
        }
    }
}
