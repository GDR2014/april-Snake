using UnityEngine;

public class GameManager : MonoBehaviour {

    public float MinDelay = .15f, MaxDelay = 1.5f;
    public float StepDelay = 1.0f;
    public int Score = 0;

    void Start () {}
    
    void Update () {}

    public void GameOver( World world, Vector2 collisionPoint ) {
        world[collisionPoint].renderer.material.color = Color.red;
        Debug.Break();
    }
}
