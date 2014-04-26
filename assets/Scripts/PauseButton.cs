using UnityEngine;

public class PauseButton : MonoBehaviour {

    public GameManager GameManager;
    private GUITexture _tex;

    void Awake() { _tex = guiTexture; }

    void Update() {
        if( !Input.GetMouseButtonUp( 0 ) ) return;
        if( _tex.HitTest( Input.mousePosition ) ) {
            GameManager.shouldStep = false;
        }
    }
}
