using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float speed = 5f;
    public float resetPositionY = -10f;
    public float startPositionY = 10f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Reset the road tile when it goes out of view
        if (transform.position.y <= resetPositionY)
        {
            transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z);
        }
    }
}