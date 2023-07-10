using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f;

    private Vector3 direction;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;
    private void Start()
    {
        CalculateBounds();
    }
    void Update()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;
        WrapAroundScene();
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    private void WrapAroundScene()
    {
        Vector3 position = transform.position;

        if (position.x < bottomLeft.x)
        {
            position.x = topRight.x;
        }
        else if (position.x > bottomRight.x)
        {
            position.x = topLeft.x;
        }

        if (position.y > topLeft.y)
        {
            position.y = bottomLeft.y;
        }
        else if (position.y < bottomLeft.y)
        {
            position.y = topLeft.y;
        }

        transform.position = position;
    }
    private void CalculateBounds()
    {
        float cameraDistance = transform.position.z - Camera.main.transform.position.z;
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraDistance));
        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraDistance));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));
    }
}


