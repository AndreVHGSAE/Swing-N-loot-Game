using UnityEngine;

public class EnergyRegain : MonoBehaviour
{
    public Animator anim;
    
    public int value = 1;
    public bool isGone = false;
    public float RespawnTimer = 2;
    public float RespawnTimerCounter;
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isGone)
        {
            collision.gameObject.GetComponent<PlayerController>().energy += value;
            isGone = true;
        }
    }

    private void Update()
    {
        anim.SetBool("isGone", isGone);
        if (isGone)
        {
            RespawnTimerCounter += Time.deltaTime;
            if(RespawnTimerCounter >= RespawnTimer)
            {
                isGone = false;
                RespawnTimerCounter = 0;
            }
        }
    }
}
