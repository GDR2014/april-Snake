using Assets.Scripts;
using UnityEngine;

public class SwipeController : MonoBehaviour {

    public float SWIPE_DISTANCE_THRESHOLD = 65.0f;
    public World world;

    private Vector3 swipe = new Vector2();

    void Update() {
        asTouch();
    }

    private void asMouse() {
        if( Input.GetMouseButtonDown( 0 ) ) swipe = Input.mousePosition;
        if( !Input.GetMouseButtonUp( 0 ) ) return;
        swipe = Input.mousePosition - swipe;
        Debug.Log( swipe );
        Snake.Direction? dir = getDirection();
        Debug.Log( dir );
    }

    private void asTouch() { //if( Input.touchCount == 0 ) return;
        if(Input.touchCount == 0 ) return;
        Touch touch = Input.GetTouch( 0 );
        if( touch.phase == TouchPhase.Began )  swipe = touch.position;
        if( touch.phase != TouchPhase.Ended ) return;

        swipe = (Vector3)touch.position - swipe;
        Debug.Log( swipe );
        Snake.Direction? dir = getDirection();
        Debug.Log( dir );
        if( dir == null ) return;

        world.turnPlayer( (Snake.Direction) dir );

    }

    private Snake.Direction? getDirection() {
        float absX = Mathf.Abs( swipe.x );
        float absY = Mathf.Abs( swipe.y );
        if( !( absX >= SWIPE_DISTANCE_THRESHOLD || absY >= SWIPE_DISTANCE_THRESHOLD ) ) return null;
        if( absX > absY ) return swipe.x > 0 ? Snake.Direction.RIGHT : Snake.Direction.LEFT;
        return swipe.y > 0 ? Snake.Direction.UP : Snake.Direction.DOWN;
    }
}
