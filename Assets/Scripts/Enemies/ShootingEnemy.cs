using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : BasicEnemy
{
    [Header("Shooting Info")]
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float BulletCooldown = 2;
    public float BulletCooldownTimer = 0;
    
    public override void ChasePlayer()
    {
        BulletCooldownTimer += Time.deltaTime;
        if (BulletCooldownTimer >= BulletCooldown)
        {
            GameObject temp = Instantiate(bullet, bulletSpawnPoint.position, transform.rotation);
            temp.SetActive(true);
            temp.transform.position = bulletSpawnPoint.position;
            temp.transform.rotation = transform.rotation;
            Rigidbody2D rbtemp = temp.GetComponent<Rigidbody2D>();
            Vector2 playerDirection = (Player.transform.position - temp.transform.position).normalized;
            playerDirection.y = transform.position.y;
            rbtemp.AddForce(playerDirection * 10, ForceMode2D.Impulse);
            BulletCooldownTimer = 0;
        }
    }
}
