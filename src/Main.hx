package ;

import SnakeWorld;
import flash.display.Sprite;
import flash.events.Event;
import flash.Lib;

using Snake.Direction;

typedef K = flash.ui.Keyboard; 

class Main extends Sprite 
{
	var inited:Bool;
	var world:SnakeWorld;
	
	static var delay = 1000;
	
	var cellSize : Float;
	
	var worldWidth = 25;
	var worldHeight = 25;

	/* ENTRY POINT */
	
	function resize(e : Event)
	{
		if (!inited) init();
		// else (resize or orientation change)
		var width = Lib.current.stage.width;
		var height = Lib.current.stage.height;
		var opt = e.
		trace('Resized: $width, $height');
		
	}
	
	function init() 
	{
		if (inited) return;
		inited = true;

		// (your code here)
		
		// Stage:
		// stage.stageWidth x stage.stageHeight @ stage.dpiScale
		
		// Assets:
		// nme.Assets.getBitmapData("img/assetname.jpg");
		var stage = Lib.current.stage;
		world = new SnakeWorld(25, 25);
		addEventListener(Event.ENTER_FRAME, onEnterFrame);
		stage.addEventListener(flash.events.KeyboardEvent.KEY_DOWN, onKeyDown.bind());
	}

	/* SETUP */

	public function new() 
	{
		super();	
		addEventListener(Event.ADDED_TO_STAGE, added);
	}

	function added(e) 
	{
		removeEventListener(Event.ADDED_TO_STAGE, added);
		stage.addEventListener(Event.RESIZE, resize);
		#if ios
		haxe.Timer.delay(init, 100); // iOS 6
		#else
		init();
		#end
	}
	
	public static function main() 
	{
		// static entry point
		Lib.current.stage.align = flash.display.StageAlign.TOP_LEFT;
		Lib.current.stage.scaleMode = flash.display.StageScaleMode.NO_SCALE;
		Lib.current.addChild(new Main());
	}
	
	public static function time()
	{
		return Lib.getTimer();
	}
	
	public function handleInput(key : UInt):Void 
	{
		if (key == K.UP) world.player.turn(Snake.Direction.Up);
		if (key == K.DOWN) world.player.turn(Direction.Down);
		if (key == K.LEFT) world.player.turn(Direction.Left);
		if (key == K.RIGHT) world.player.turn(Direction.Right);
	}
	
	public function drawBoard(grid : Array<Array<Cell>>):Void 
	{
		for (row in 0...grid.length) 
		{
			for ( col in 0...grid[row].length )
			{
				
			}
		}
	}
	/* Events */
	
	var nextUpdate = time() + delay;
	private function onEnterFrame (event:Event):Void {
		var now = time();
		if ( now < nextUpdate ) return;
		world.step();
		nextUpdate = now + delay;
		trace(world);
		drawBoard(world.grid);
	}
	
	private function onKeyDown( e : flash.events.KeyboardEvent ):Void 
	{
		trace(e.keyCode);
		handleInput(e.keyCode);
	}
}
