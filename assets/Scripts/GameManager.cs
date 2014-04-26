using System.Security.Policy;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float MinDelay = .15f, MaxDelay = 1.5f;
    public float StepDelay = 1.2f;

    private int _score;
    public int Score{ 
        get { return _score; }
        set {
            _score = value;
            ScoreText.text = "Score: " + _score;
        }
    }

    private int _highScore;
    public int HighScore {
        get { return _highScore; }
        set {
            _highScore = value;
            HighScoreText.text = "Best: " + _highScore;
            PlayerPrefs.SetInt( "highscore", value );
        }
    }

    public GUIText ScoreText;
    public GUIText HighScoreText;

    public bool shouldSpawnPellet = true;
    public bool shouldStep = false;
    public bool shouldSkip = true;
    public bool isGameOver = false;

    void Start() {
        HighScore = PlayerPrefs.GetInt( "highscore" );
    }

    public void GameOver( World world, Vector2 collisionPoint ) {
        isGameOver = true;
        shouldStep = false;
        world[collisionPoint].renderer.material.color = Color.red;
        if( Score > HighScore ) HighScore = Score;
    }
}
