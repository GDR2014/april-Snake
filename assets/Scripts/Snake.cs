using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    public class Snake : World.IWorldStepper{

        public enum Direction { UP, DOWN, LEFT, RIGHT }

        private Direction currentDirection, nextDirection;
        private Vector2 headPos;
        private Queue<Tile> snakeSegments; // TODO: Consider changing to a queue of Vectors
        private int supposedSize;

        public Snake( Vector2 startPos, Direction startDir, int startSize, World world ) {
            snakeSegments = new Queue<Tile>();
            headPos = startPos;
            snakeSegments.Enqueue(world[headPos]);
            supposedSize = startSize;
            nextDirection = currentDirection = startDir;
        }

        public bool TurnTo( Direction dir ) {
            bool targetDirIsVertical = isDirectionVertical( dir );
            bool currentDirIsVertical = isDirectionVertical( currentDirection );
            if( !(currentDirIsVertical ^ targetDirIsVertical) ) return false; // If they're not different
            nextDirection = dir;
            return true;
        }

        public void OnStep( World world ) {
            currentDirection = nextDirection;
            Vector2 previousPosition = headPos;
            switch( currentDirection ) {
                case Direction.UP:    headPos.y--; break;
                case Direction.DOWN:  headPos.y++; break;
                case Direction.LEFT:  headPos.x--; break;
                case Direction.RIGHT: headPos.x++; break;
                default: throw new ArgumentOutOfRangeException();
            }
            bool withinBounds = checkBounds( world );
            if ( !withinBounds ) { world.GameManager.GameOver( world, previousPosition ); return; }
            Tile oldHead = world[previousPosition];
            Tile newHead = world[headPos];
            snakeSegments.Enqueue( newHead );
            // oldHead.changeTypeTo(Cell.Body);
            oldHead.renderer.material.color = GameProperties.COLOR_CELL_BODY;
            newHead.changeTypeTo(Cell.Head);
            bool shouldGrow = snakeSegments.Count < supposedSize;
            if( shouldGrow ) return;
            Tile tail = snakeSegments.Dequeue();
            tail.changeTypeTo(Cell.Empty);
        }

        private bool checkBounds(World world) {
            return !(headPos.x < 0 || headPos.x >= world.width
                  || headPos.y < 0 || headPos.y >= world.height);
        }

        private bool isDirectionVertical( Direction dir ) {
            return dir == Direction.UP || dir == Direction.DOWN;
        }
    }

}