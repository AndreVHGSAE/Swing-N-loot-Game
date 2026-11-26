using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Graple : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerController player;
    float lifeTime = 2;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(PlayerController p_PLayer)
    {
        player = p_PLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
