using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float currentTime;
    public float maxTime = 2;

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxTime)
            {
                Object.Destroy(this.gameObject);
                currentTime = 0;
            }
        }
    }
}
