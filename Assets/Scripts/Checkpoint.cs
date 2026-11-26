using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Checkpoint : MonoBehaviour
{
    public Animator anim;
    PlayerController Player;
    public bool IsActive = false;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !IsActive)
        {
            collision.gameObject.GetComponent<PlayerController>().SetRespawn();
            IsActive = true;
            anim.SetBool("IsActive", IsActive);
        }
    }
}
