using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float Xoffset = 1.1f;
    public float Yoffset = 1.1f;

    void Update()
    {
        transform.position = new Vector3((player.transform.position.x*Xoffset), (player.transform.position.y*Yoffset), transform.position.z);
    }
}

