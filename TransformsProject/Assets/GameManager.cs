
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Ball ball;
    public Wall wall;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;
    public Vector2 force = Vector2.up;

    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
   
    }
}
