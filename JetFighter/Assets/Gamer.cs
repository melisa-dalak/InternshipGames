using UnityEngine;
public enum GamerType
{
    Gamer,
    Enemy
}

public class Gamer : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 point;

    public float RotationSpeed = 1f;
    public float zRotation = 45f;

    public GamerType gamerType;

    public GameObject gamerBulletPrefab;
    public GameObject enemyBulletPrefab;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;

    private GameManager gameManager;
    
    void Start()
    {
        CalculateBounds();
        gameManager = FindObjectOfType<GameManager>();

        gameManager.UpdateScoreText();
    }

    void Update()
    {
        Vector3 initialDirection = Vector3.up;

        Keycodes();

        Vector3 forwardDirection = GetForwardDirection(zRotation, initialDirection);

        transform.up = forwardDirection;
        transform.position += forwardDirection * speed * Time.deltaTime;

        WrapAroundScene();
    }

    public Vector3 GetForwardDirection(float angle, Vector3 initialDirection)
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

    private void Keycodes()
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
                SpawnBullet(gamerBulletPrefab);
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
                SpawnBullet(enemyBulletPrefab);
            }
        }
    }

    private void CalculateBounds()
    {
        float cameraDistance = transform.position.z - Camera.main.transform.position.z;
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraDistance));
        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraDistance));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));
    }

    private void SpawnBullet(GameObject bulletPrefab)
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
        int scoreGamer = 0;
        int scoreEnemy = 0;
       

        if (gamerType == GamerType.Gamer && collision.gameObject.CompareTag("enemyBulletPrefab"))
        {
            scoreGamer++;
            if (gameManager != null)
            {
                gameManager.IncreaseScoreEnemy();
            }
            Destroy(collision.gameObject);
        }
        else if (gamerType == GamerType.Enemy && collision.gameObject.CompareTag("gamerBulletPrefab"))
        {
            scoreEnemy++;
            if (gameManager != null)
            {
                gameManager.IncreaseScoreGamer();
            }
            Destroy(collision.gameObject);
        }
        if (scoreEnemy >= 5 || scoreGamer >= 5)
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        Time.timeScale = 0f;
        gameManager.UpdateScoreText();
    }
}

