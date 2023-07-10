
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public Gamer gamer;
    public Text scoreText;

    public int scoreGamer = 0;
    public int scoreEnemy = 0;

    public void IncreaseScoreGamer()
    {
        scoreGamer++;
        UpdateScoreText();
    }

    public void IncreaseScoreEnemy()
    {
        scoreEnemy++;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Gamer score: " + scoreGamer + "\nEnemy score: " + scoreEnemy;
    }
}
