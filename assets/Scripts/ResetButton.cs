using UnityEngine;

public class ResetButton : MonoBehaviour {

    private GUITexture _tex;

    void Awake() { _tex = guiTexture; }

    void Update() {
        if ( !Input.GetMouseButtonUp( 0 ) ) return;
        if ( _tex.HitTest( Input.mousePosition ) ) {
            Application.LoadLevel(0);
        }
    }
}
