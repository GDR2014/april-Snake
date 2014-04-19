using UnityEngine;

public class GameManager : MonoBehaviour {

    public float StepDelay = 1.0f;

    void Start () {}
    
    void Update () {}

    public void GameOver( World world, Vector2 collisionPoint ) {
        world[collisionPoint].renderer.material.color = Color.red;
        Debug.Break();
    }
}
