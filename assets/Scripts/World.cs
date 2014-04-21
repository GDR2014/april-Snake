using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

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
        startStepping();
    }

    private void Update() {
        if ( !GameManager.isGameOver && !GameManager.shouldStep && Input.anyKeyDown ) GameManager.shouldStep = true;
        // The world really shouldn't move the player, but bleh...
        if( GameManager.isGameOver ) return;
        if ( Input.GetKeyDown( KeyCode.W ) ) turn(Snake.Direction.UP);
        if ( Input.GetKeyDown( KeyCode.S ) ) turn( Snake.Direction.DOWN );
        if ( Input.GetKeyDown( KeyCode.A ) ) turn( Snake.Direction.LEFT );
        if ( Input.GetKeyDown( KeyCode.D ) ) turn( Snake.Direction.RIGHT );
    }

    private void turn( Snake.Direction dir ) {
        bool hasTurned = Player.TurnTo(dir);
        if( !hasTurned ) return;
        stopStepping();
        NotifySteppers();
        startStepping();
    }

    public IEnumerator StepRoutine() {
        yield return new WaitForSeconds( GameManager.StepDelay );
        if(GameManager.shouldStep) NotifySteppers();
        spawnPellet();
        startStepping();
    } 

    public interface IWorldStepper {
        void OnStep( World world );
    }

    public void NotifySteppers() {
        for( int i = 0; i < Steppers.Count; i++ )
            Steppers[i].OnStep(this);
    }

    private void spawnPellet() {
        if( !GameManager.shouldSpawnPellet ) return;
        GameManager.shouldSpawnPellet = false;
        List<Tile> emptyTiles = new List<Tile>();
        for( int row = 0; row < grid.Length; row++ ) {
            for( int col = 0; col < grid[row].Length; col++ ) {
                Tile t = this[row, col];
                if( t.Type == Cell.Empty ) emptyTiles.Add(t);
            }
        }
        int idx = Random.Range( 0, emptyTiles.Count );
        emptyTiles[idx].changeTypeTo(Cell.Pellet);
    }

    private void startStepping() {
        StartCoroutine( "StepRoutine" );
    }

    private void stopStepping() {
        StopCoroutine("StepRoutine");
    }
}