using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgObstacle : MonoBehaviour
{
    PlayerController Player;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Died();
        }
    }
}
