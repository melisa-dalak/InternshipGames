using UnityEngine;
public enum GamerType
{
    Gamer,
    Enemy
}
public class Gamer : MonoBehaviour
{
    public float speed;
    public float RotationSpeed = 1f;
    public float zRotation = 45f;
    public float BulletCreateSpeed;

    public GamerType gamerType;

    public GameObject gamerBulletPrefab;
    public GameObject enemyBulletPrefab;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;

    public Vector3 point;

    private GameManager gameManager;
    public Timer inputTimer;
    private Animation animationComponent;

    void Start()
    {
        CalculateBounds();

        gameManager = FindObjectOfType<GameManager>();

        gameManager.UpdateScoreText();

        inputTimer = new Timer(1f);
        inputTimer.ForceComplete();
       
        animationComponent = GetComponent<Animation>();
    }

    void Update()
    {
        Vector3 initialDirection = Vector3.up;

        Keycodes();
        inputTimer.Update();

        Vector3 forwardDirection = GetForwardDirection(zRotation, initialDirection);

        transform.up = forwardDirection;
        transform.position += forwardDirection * speed * Time.deltaTime;

        WrapAroundScene();
    }

    public Vector3 GetForwardDirection(float angle, Vector3 initialDirection)  //Z rotation
    {
        float radians = angle * Mathf.Deg2Rad;
        float forwardX = initialDirection.x * Mathf.Cos(radians) - initialDirection.y * Mathf.Sin(radians);
        float forwardY = initialDirection.y * Mathf.Cos(radians) + initialDirection.x * Mathf.Sin(radians);
        return new Vector3(forwardX, forwardY, 0f).normalized;
    }

    private void WrapAroundScene()   //Continuous Area
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

    private void Keycodes() //Key working
    {
        float RotationStandard = RotationSpeed * Time.deltaTime;

        if (gamerType == GamerType.Gamer)
        {
            if (Input.GetKey(KeyCode.A))
            {
                zRotation += RotationStandard;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                zRotation -= RotationStandard;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (inputTimer.isDone()) 
                {
                    inputTimer.ReStart();
                    SpawnBullet(gamerBulletPrefab);
                }
                
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

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (inputTimer.isDone())
                {
                    inputTimer.ReStart();
                    SpawnBullet(enemyBulletPrefab);
                }
            }
        }
    }

    private void CalculateBounds() //Area Calculating
    {
        float cameraDistance = transform.position.z - Camera.main.transform.position.z;
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraDistance));
        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraDistance));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));
    }

    private void SpawnBullet(GameObject bulletPrefab) //create bullet
    {
        GameObject currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
      
        Bullet bulletComponent = currentBullet.GetComponent<Bullet>();
       
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(transform.up);
        }
        Destroy(currentBullet, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gamerType == GamerType.Gamer && collision.gameObject.CompareTag("enemyBulletPrefab"))
        {
            if (gameManager != null)
            {
                gameManager.IncreaseScoreEnemy();
            }

            Destroy(collision.gameObject);
            animationComponent.PlayAnimation();
        }
        else if (gamerType == GamerType.Enemy && collision.gameObject.CompareTag("gamerBulletPrefab"))
        {
            if (gameManager != null)
            {
                gameManager.IncreaseScoreGamer();
            }

            Destroy(collision.gameObject);
            animationComponent.PlayAnimation();
        }
    }
}