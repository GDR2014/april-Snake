using UnityEngine;

public class GameManager : MonoBehaviour {

    public float MinDelay = .15f, MaxDelay = 1.5f;
    public float StepDelay = 1.0f;
    public int Score = 0;

    public bool shouldSpawnPellet = true;
    public bool shouldStep = false;
    public bool shouldSkip = false;
    public bool isGameOver = false;

    void Start () {}
    
    void Update () {}

    public void GameOver( World world, Vector2 collisionPoint ) {
        isGameOver = true;
        shouldStep = false;
        world[collisionPoint].renderer.material.color = Color.red;
    }
}
