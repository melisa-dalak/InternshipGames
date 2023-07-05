using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    private float initialGizmoRotation = 0f;

    public Paddle paddleRight;
    public Paddle paddleLeft;

    Vector2 direction;

    private int scorePaddleRight = 0;
    private int scorePaddleLeft = 0;

    private bool isGameStarted;

    public Text showScore;

    void Start()
    {
        direction = Vector2.one;

        paddleRight = GameObject.FindGameObjectWithTag("PaddleRight").GetComponent<Paddle>();
        paddleLeft = GameObject.FindGameObjectWithTag("PaddleLeft").GetComponent<Paddle>();

        direction = Quaternion.Euler(0f, 0f, initialGizmoRotation) * Vector2.right; //gizmo çizimi

        if (!isGameStarted)  //Top gizmo açýsýnda harekete baþlar
        {
            float rotationAngle = initialGizmoRotation * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle)).normalized;
            isGameStarted = true;
        }
    }

    void Update()
    {
        if (transform.position.y < GameManager.bottomLeft.y && direction.y < 0) //alanýn sýnýrlarý
        {
            direction.y = -direction.y;
        }
        if (transform.position.y > GameManager.topRight.y && direction.y > 0)
        {
            direction.y = -direction.y;
        }

        Vector2 velocity = direction * speed * Time.deltaTime;
        Vector2 targetPosition = transform.position + new Vector3(velocity.x, velocity.y, 0f);


        Vector2 intersection;
        if (CheckLineLineIntersection(paddleRight.transform.position - paddleRight.transform.up, paddleRight.transform.position + paddleRight.transform.up, transform.position, targetPosition, out intersection))
        { 
            intersection = intersection + ((Vector2)transform.position - intersection).normalized * 0.001f;
            targetPosition = intersection;
            direction.x = -direction.x;
        }
        else if (CheckLineLineIntersection(paddleLeft.transform.position - paddleLeft.transform.up, paddleLeft.transform.position + paddleLeft.transform.up, transform.position, targetPosition, out intersection))
        {
            intersection = intersection + ((Vector2)transform.position - intersection).normalized * 0.001f;
            targetPosition = intersection;
            direction.x = -direction.x;
        }

        transform.position = targetPosition;

        Score();

    }

    private void ResetBall()  // top sahadan çýktýktan sonra resetlensin
    {
        transform.position = Vector3.zero;
        direction = Vector2.one;
        isGameStarted = false;
    }


    private bool CheckLineLineIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End, out Vector2 intersectionPoint) //line intersection metodunu burada oluþturuyoruz.
    {
        intersectionPoint = Vector2.zero;
        float denominator = ((line2End.y - line2Start.y) * (line1End.x - line1Start.x)) -
                            ((line2End.x - line2Start.x) * (line1End.y - line1Start.y));

        if (denominator == 0)
        {
            return false;
        }

        float ua = (((line2End.x - line2Start.x) * (line1Start.y - line2Start.y)) -
                    ((line2End.y - line2Start.y) * (line1Start.x - line2Start.x))) / denominator;

        float ub = (((line1End.x - line1Start.x) * (line1Start.y - line2Start.y)) -
                    ((line1End.y - line1Start.y) * (line1Start.x - line2Start.x))) / denominator;

        if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
        {
            intersectionPoint = Vector2.Lerp(line1Start, line1End, ua);
            return true;
        }

        return false;
    }

    private void Score()
    {
        if (transform.position.x < GameManager.bottomLeft.x) //top alanýn solundan çýkarsa saðdaki oyuncu 1 puan kazanýr
        {
            scorePaddleRight++;
            showScore.text = "Right Player Score:" + scorePaddleRight + "  \nLeft Player Score: " + scorePaddleLeft;
            ResetBall();
        }
        else if (transform.position.x > GameManager.topRight.x) // top alanýn saðýndan çýkarsa soldaki oyuncu 1 puan kazanýr.
        {
            scorePaddleLeft++;
            showScore.text = "Right Player Score: " + scorePaddleRight + "  \nLeft Player Score: " + scorePaddleLeft;
            ResetBall();
        }


        if (scorePaddleLeft >= 10 || scorePaddleRight >= 10)    //skoru 10 yapan kazanýr.
        {
            EndGame();
        }

    }
    private void EndGame()
    {
        if (scorePaddleLeft > scorePaddleRight)
        {
            showScore.text = "Game Over! LEFT PLAYER WÝN \nLeft Player Score: " + scorePaddleLeft + "\nRight Player Score: " + scorePaddleRight;
        }
        else if (scorePaddleRight > scorePaddleLeft)
        {
            showScore.text = "Game Over! RÝGHT PLAYER WÝN \nLeft Player Score: " + scorePaddleLeft + "\nRight Player Score: " + scorePaddleRight;
        }
        else
        {
            showScore.text = "Game Over! Berabere! \nLeft Player Score: " + scorePaddleLeft + "\nRight Player Score: " + scorePaddleRight;
        }
        Time.timeScale = 0f;
    }

}
