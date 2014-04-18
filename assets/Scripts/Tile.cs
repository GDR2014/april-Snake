using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

// The visual representation of a cell
[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour {

    public Cell Type;
    public Vector3 StartSize;

    void Start() {
        StartSize = transform.localScale;
        renderer.material.color = GameProperties.GetCellColor( Type );
    }
    
    void Update () {}

    public void changeTypeTo( Cell cell ) {
        StopAllCoroutines();
        StartCoroutine( changeTypeCoroutine( cell ) );
    }

    private IEnumerator changeTypeCoroutine( Cell type ) {
        Type = type;
        Vector3 initialSize = transform.localScale;
        Vector3 targetSize = new Vector3(0,0,0);
        float t = 0.1f;
        while( transform.localScale.x > targetSize.x ) {
            transform.localScale = Vector3.Lerp( initialSize, targetSize, t += t * 1.04f );
            yield return null;
        }
        transform.localScale = targetSize;
        renderer.material.color = GameProperties.GetCellColor( Type );
        targetSize = StartSize;
        while( transform.localScale.x < targetSize.x - 0.005f) {
            transform.localScale = Vector3.Lerp( transform.localScale, targetSize, .4f );
            yield return null;
        }
        transform.localScale = targetSize;
    }

    void OnMouseDown() {
        Cell next;
        switch( Type ) {
            case Cell.Head:
                next = Cell.Body;
                break;
            case Cell.Body:
                next = Cell.Pellet;
                break;
            case Cell.Pellet:
                next = Cell.Wall;
                break;
            case Cell.Wall:
                next = Cell.Empty;
                break;
            case Cell.Empty:
                next = Cell.Head;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        changeTypeTo(next);
    }
}
