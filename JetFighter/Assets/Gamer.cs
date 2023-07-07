using UnityEngine;

public class Gamer : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 point;

    public float RotationSpeed = 1f;
    public float zRotation = 45f;

    public GamerType gamerType;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;

        public GameObject bulletPrefab;

    void Start()
    {
        CalculateBounds();
    }

    void Update()
    {
        Vector3 initialDirection = Vector3.up;
        float RotationStandard = RotationSpeed * Time.deltaTime;

      if(gamerType == GamerType.Gamer) 
      { 
            if (Input.GetKey(KeyCode.A))
            {
                zRotation += RotationStandard;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                zRotation -= RotationStandard;
            }
      }
       else if (gamerType == GamerType.Enemy)
       { 
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                zRotation += RotationStandard;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                zRotation -= RotationStandard;
            }
       }

        Vector3 forwardDirection = GetForwardDirection(zRotation, initialDirection);
  
        transform.up = forwardDirection;
        transform.position += forwardDirection * speed * Time.deltaTime;

        WrapAroundScene();
       

    }

   
    public Vector3 GetForwardDirection(float angle,Vector3 initialDirection)
    {
        float radians = angle * Mathf.Deg2Rad;
        float forwardX = initialDirection.x * Mathf.Cos(radians) - initialDirection.y * Mathf.Sin(radians);
        float forwardY = initialDirection.y * Mathf.Cos(radians) + initialDirection.x * Mathf.Sin(radians);
        return new Vector3(forwardX, forwardY, 0f).normalized;
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

public enum GamerType
    {
        Gamer,
        Enemy
    }