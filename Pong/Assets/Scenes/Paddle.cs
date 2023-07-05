using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PaddleType //paddlelarý tanýmlamak için
{
    PaddleRight,
    PaddleLeft
}

public class Paddle : MonoBehaviour
{
    [SerializeField]
    public float speed;

    float height;

    public PaddleType paddleType;
    [SerializeField]
    private string Vertical = "Vertical"; 

    [SerializeField]
    private string Vertical2 = "Vertical2";
    void Start()
    {
        height = transform.localScale.y; 
        speed = 5f;
    }

 

    void Update()

        //paddle hareketleri
    {
        float moveVertical = 0f;

        if (paddleType == PaddleType.PaddleRight)
        {
            moveVertical = Input.GetAxisRaw(Vertical);
        }
        else if (paddleType == PaddleType.PaddleLeft)
        {
            moveVertical = Input.GetAxisRaw(Vertical2);
        }

        transform.Translate(Vector3.up * moveVertical * speed * Time.deltaTime);

        if (transform.position.y < GameManager.bottomLeft.y + height / 2)
        {
            transform.position = new Vector3(transform.position.x, GameManager.bottomLeft.y + height / 2, transform.position.z);
        }
        else if (transform.position.y > GameManager.topRight.y - height / 2)
        {
            transform.position = new Vector3(transform.position.x, GameManager.topRight.y - height / 2, transform.position.z);
        }


    }

  

}
