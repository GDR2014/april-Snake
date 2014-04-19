using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class World : MonoBehaviour {

    public List<IWorldStepper> Steppers; // Lowest index gets updated first

    public GameManager GameManager;

    public Snake Player;
    public Vector2 StartPos = new Vector2(2, 5);
    public Snake.Direction StartDir = Snake.Direction.RIGHT;
    public int StartSize = 4;

    public Tile tilePrefab;
    public Vector2 tileSpacing = new Vector2(0,0);

    public int width, height;
    private Tile[][] grid;
    public Tile this[ int x, int y ] {
        get { return grid[y][x]; }
        set { grid[y][x] = value; }
    }

    public Tile this[ Vector2 pos ] { get { return this[(int) pos.x, (int) pos.y]; } }

    private void Awake() {
        Steppers = new List<IWorldStepper>();
    }

    private void Start() {
        Vector3 cellDimension = tilePrefab.transform.localScale;
        float offsetX = cellDimension.x + tileSpacing.x;
        float offsetY = cellDimension.y + tileSpacing.y;
        float initialX = (-width  / 2.0f) * offsetX + offsetX/2; // Minus
        float initialY = (+height / 2.0f) * offsetY - offsetY/2; // Plus
        grid = new Tile[height][];
        Debug.Log( "Grid initialized!" );
        for( int row = 0; row < grid.Length; row++ ) {
            grid[row] = new Tile[width];
            for( int col = 0; col < grid[row].Length; col++ ) {
                float posX = initialX + col * offsetX; // Plus
                float posY = initialY - row * offsetY; // Minus
                Vector3 pos = new Vector3( posX, posY );
                Tile tile = (Tile) Instantiate( tilePrefab, pos, Quaternion.identity );
                tile.transform.parent = transform;
                tile.name = String.Format( "Tile ({0}, {1})", col, row );
                grid[row][col] = tile;
            }
        }
        Player = new Snake( StartPos, StartDir, StartSize, this );
        Steppers.Add( Player );
        StartCoroutine( StepRoutine() );
    }

    private void Update() {
        // The world really shouldn't move the player, but bleh...
        if ( Input.GetKeyDown( KeyCode.W ) ) turn(Snake.Direction.UP);
        if ( Input.GetKeyDown( KeyCode.S ) ) turn( Snake.Direction.DOWN );
        if ( Input.GetKeyDown( KeyCode.A ) ) turn( Snake.Direction.LEFT );
        if ( Input.GetKeyDown( KeyCode.D ) ) turn( Snake.Direction.RIGHT );
    }

    private void turn( Snake.Direction dir ) {
        bool hasTurned = Player.TurnTo(dir);
        Debug.Log("Should turn? " + hasTurned);
        if( !hasTurned ) return;
        StopAllCoroutines();
        NotifySteppers();
        StartCoroutine( StepRoutine() );
    }

    public IEnumerator StepRoutine() {
        yield return new WaitForSeconds( GameManager.StepDelay );
        NotifySteppers();
        StartCoroutine( StepRoutine() );
    } 

    public interface IWorldStepper {
        void OnStep( World world );
    }

    public void NotifySteppers() {
        for( int i = 0; i < Steppers.Count; i++ )
            Steppers[i].OnStep(this);
    }
}