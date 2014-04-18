using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public HashSet<IWorldStepper> Steppers;
 
    public Tile tilePrefab;
    public Vector2 tileSpacing = new Vector2(0,0);

    public int width, height;
    private Tile[][] grid;
    public Tile this[ int x, int y ] {
        get { return grid[x][y]; }
        set { grid[x][y] = value; }
    }

    private void Awake() {
        Steppers = new HashSet<IWorldStepper>();
        // Steppers.add( new Snake() );
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
            Debug.Log( "Row " + row + " created!" );
        }
    }

    private void Update() {}


    public interface IWorldStepper {
        void OnStep( World world );
    }
}