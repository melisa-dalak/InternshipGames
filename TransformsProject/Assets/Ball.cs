using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public float radius;

    public Wall wall;

    private Vector2 direction;

    private void Start()
    {
        direction = Vector2.zero;
    }

    private void Update()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
            direction.x--;
        else if (Input.GetKey(KeyCode.D))
            direction.x++;

        if (Input.GetKey(KeyCode.W))
            direction.y++;
        else if (Input.GetKey(KeyCode.S))
            direction.y--;

        direction = direction.normalized;

        Vector2 velocity = direction * speed * Time.deltaTime;
        Vector2 targetPosition = transform.position + new Vector3(velocity.x, velocity.y, 0f);

        targetPosition = ClampPosition(targetPosition);

        Vector2 intersectionPoint;
        if (CheckLineLineIntersection(wall.point1.position, wall.point2.position, transform.position, targetPosition, out intersectionPoint)) 
        {
            intersectionPoint = intersectionPoint + ((Vector2)transform.position - intersectionPoint).normalized * 0.001f ;
            targetPosition = intersectionPoint;
        }

        transform.position = targetPosition;
    }
    
    private Vector2 ClampPosition(Vector2 position) //alanýn sýnýrlarý
    {
        float clampedX = Mathf.Clamp(position.x, GameManager.bottomLeft.x, GameManager.topRight.x);
        float clampedY = Mathf.Clamp(position.y, GameManager.bottomLeft.y, GameManager.topRight.y);
        return new Vector2(clampedX, clampedY);
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
}
