using UnityEngine;

public class GameManager : MonoBehaviour {

    public float MinDelay = .15f, MaxDelay = 1.5f;
    public float StepDelay = 1.0f;

    private int _score;
    public int Score{ 
        get { return _score; }
        set {
            _score = value;
            ScoreText.text = "Score: " + _score;
        }
    }

    public GUIText ScoreText;

    public bool shouldSpawnPellet = true;
    public bool shouldStep = false;
    public bool shouldSkip = false;
    public bool isGameOver = false;

    public void GameOver( World world, Vector2 collisionPoint ) {
        isGameOver = true;
        shouldStep = false;
        world[collisionPoint].renderer.material.color = Color.red;
    }
}
