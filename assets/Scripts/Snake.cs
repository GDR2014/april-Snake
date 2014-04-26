using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts {

    public class Snake : World.IWorldStepper {

        public enum Direction { UP, DOWN, LEFT, RIGHT }

        private Direction currentDirection, nextDirection;
        private Vector2 headPos;
        private Queue<Tile> snakeSegments; // TODO: Consider changing to a queue of Vectors
        private int supposedSize;

        private World world;

        public Snake( Vector2 startPos, Direction startDir, int startSize, World world ) {
            snakeSegments = new Queue<Tile>();
            headPos = startPos;
            snakeSegments.Enqueue(world[headPos]);
            world[headPos].changeTypeTo(Cell.Head);
            supposedSize = startSize;
            nextDirection = currentDirection = startDir;
            this.world = world;
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
            Vector2 newHeadPos = headPos;
            Vector2 previousPosition = headPos;
            switch( currentDirection ) {
                case Direction.UP:    newHeadPos.y--; break;
                case Direction.DOWN: newHeadPos.y++; break;
                case Direction.LEFT: newHeadPos.x--; break;
                case Direction.RIGHT: newHeadPos.x++; break;
                default: throw new ArgumentOutOfRangeException();
            }
            Cell cellAtHead = checkNewHeadPosition( newHeadPos );
            bool isGameOver = !handleNewCell(cellAtHead);
            if( isGameOver && world.GameManager.shouldSkip ) {
                world.GameManager.shouldSkip = false; return;}
            if( isGameOver ) {
                world.GameManager.GameOver(world, previousPosition); 
                return;
            }
            headPos = newHeadPos;
            world.GameManager.shouldSkip = true;
            Tile oldHead = world[previousPosition];
            Tile newHead = world[headPos];
            snakeSegments.Enqueue( newHead );
            oldHead.changeTypeTo(Cell.Body, false);
            newHead.changeTypeTo(Cell.Head, false);
            bool shouldGrow = snakeSegments.Count < supposedSize;
            if( shouldGrow ) return;
            Tile tail = snakeSegments.Dequeue();
            tail.changeTypeTo(Cell.Empty);
        }

        private Cell checkNewHeadPosition(Vector2 headPos) {
            if( headPos.x < 0 || headPos.x >= world.width
               || headPos.y < 0 || headPos.y >= world.height) return Cell.Wall;
            return world[headPos].Type;
        }

        // Returns true if the next cell is valid; false if it's invalid
        private bool handleNewCell( Cell c ) {
            switch( c ) {
                case Cell.Head:
                case Cell.Body:
                case Cell.Wall:
                    return false;
                case Cell.Pellet:
                    eatPellet();
                    break;
                case Cell.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "c" );
            }
            return true;
        }

        private bool isDirectionVertical( Direction dir ) {
            return dir == Direction.UP || dir == Direction.DOWN;
        }

        private void eatPellet() {
            world.GameManager.Score++;
            world.GameManager.StepDelay = world.GameManager.StepDelay * .9f < world.GameManager.MinDelay ? world.GameManager.MinDelay : world.GameManager.StepDelay * .9f; // TODO: Fix this.
            supposedSize++;
            world.GameManager.shouldSpawnPellet = true;
        }
    }

}