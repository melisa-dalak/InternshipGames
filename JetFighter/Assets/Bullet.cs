
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float bullet_speed;
    public float radius;

    public Gamer gamer;
    public Gamer enemy;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;
    private Vector2 topLeft;
    private Vector2 topRight;

    public BulletType bulletType;

    private Vector2 direction;
    private bool isFired;


    public Text scoreText;
    public int scoreGamer = 0;
    public int scoreEnemy = 0;
   
    void Start()
    {

        CalculateBounds();

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Gamer>();
        gamer = GameObject.FindGameObjectWithTag("Gamer").GetComponent<Gamer>();


        direction = Random.insideUnitCircle.normalized;
        isFired = false;

    }


    void Update()
    {
        if (bulletType == BulletType.GamerBullet && Input.GetKeyDown(KeyCode.Space) && !isFired)
        {
            isFired = true;
        }
        else if (bulletType == BulletType.EnemyBullet && Input.GetKeyDown(KeyCode.Return) && !isFired)
        {
            isFired = true;
        }

        if (isFired)
        {
            transform.position += (Vector3)direction * bullet_speed * Time.deltaTime;
            WrapAroundScene();
        }

        if (scoreGamer >= 3 || scoreEnemy >= 3)    //skoru 10 yapan kazanýr.
        {
            EndGame();
        }

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (bulletType == BulletType.GamerBullet && collision.CompareTag("Enemy"))
        {
            gameManager.IncreaseScoreGamer();
            Destroy(gameObject);

        }
        else if (bulletType == BulletType.EnemyBullet && collision.CompareTag("Gamer"))
        {
            gameManager.IncreaseScoreEnemy();
            Destroy(gameObject);
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


    public enum BulletType
    {
        GamerBullet,
        EnemyBullet
    }

    private void EndGame()
    {
        if (scoreGamer > scoreEnemy)
        {
            scoreText.text = "Game Over! GAMER WÝN \nGamer Score: " + scoreGamer + "\nEnemy Score: " + scoreEnemy;
        }
        else if (scoreEnemy > scoreGamer)
        {
            scoreText.text = "Game Over! ENEMY WÝN \nGamer Score: " + scoreGamer + "\nEnemy Score: " + scoreEnemy;
        }
        else
        {
            scoreText.text = "Game Over! BERABERE! \nGamer Score: " + scoreGamer + "\nEnemy Score: " + scoreEnemy;
        }
        Time.timeScale = 0f;
    }
}
