using System;
using UnityEngine;

namespace Assets.Scripts {

    public class GameProperties {
        public static Color
            COLOR_CELL_EMPTY  = new Color( 1.0f, 1.0f, 1.0f ),   
            COLOR_CELL_WALL   = new Color( 0.5f, 0.5f, 0.5f ),   
            COLOR_CELL_HEAD   = new Color( 0.1f, 0.5f, 0.1f ),   
            COLOR_CELL_BODY   = new Color( 0.1f, 0.7f, 0.1f ),
            COLOR_CELL_PELLET = new Color( 0.2f, 0.0f, 0.7f );


        public static Color GetCellColor( Cell cell ) {
            switch( cell ) {
                case Cell.Head:     return COLOR_CELL_HEAD;
                case Cell.Body:     return COLOR_CELL_BODY;
                case Cell.Pellet:   return COLOR_CELL_PELLET;
                case Cell.Wall:     return COLOR_CELL_WALL;
                case Cell.Empty:    return COLOR_CELL_EMPTY;
                default: throw new ArgumentOutOfRangeException( "cell" );
            }
        }
    }

}