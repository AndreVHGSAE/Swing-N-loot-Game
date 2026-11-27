using UnityEngine;

public class OnOffObstacle : DmgObstacle 
{
    public Animator anim;

    public bool isWorking;

    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownTimer;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWorking)
        {
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().Died();
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            isWorking = !isWorking;
            cooldownTimer = cooldown;
        }

        anim.SetBool("isWorking", isWorking);

        if (isWorking)
        {
            
        }
    }
}
