using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int lives = 3;
    public static int score = 0;
    public Text livesText;
    public Text scoreText;

    void Start()
    {
        livesText.text = lives.ToString();
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if(lives < 1)
        {
            lives = 3;
            score = 0;
            Enemy.rows = 2;
            Enemy.columns = 3;
            SceneManager.LoadScene(2);

        }


        livesText.text = lives.ToString();
        scoreText.text = score.ToString();
    }
}
