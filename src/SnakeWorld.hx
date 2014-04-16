package ;


using SnakeWorld.Cells;
using Snake;
using Snake.Direction;
import flash.display.Sprite;

class SnakeWorld
{
	public var grid : Array<Array<Cell>>;
	public var width(default, null) : Int;
	public var height(default, null) : Int;
	
	public var player : Snake;
	
	public function new(width : Int, height : Int) 
	{
		
		this.width = width;
		this.height = height;
		
		this.grid = [for (i in 0...height) [for (i in 0...width) Cell.Empty]];
		player = new Snake(this,0,0,Right);
		player.supposedSize = 5;
		player.move();
		player.turn(Down);
		player.move();
		player.turn(Right);
		player.move();
		player.turn(Down);
		player.move();
		player.turn(Right);
		player.move();
		
		update();
		trace("\n" + this.toString());
	}
	
	public function step():Void 
	{
		player.move();
		update();
	}
	
	public function get(x : Int, y : Int):Cell
	{
		return grid[y][x];
	}
	
	public function update() {
		clear();
		player.reportPosition(grid);
	}
	
	function clear():Void 
	{
		for ( row in 0...grid.length )
		{
			for ( col in 0...grid[row].length)
			{
				grid[row][col] = Empty;
			}
		}
	}
	
	public function toString():String 
	{
		var sb = new StringBuf();
		for ( row in 0...grid.length )
		{
			for ( col in 0...grid[row].length)
			{
				sb.add(grid[row][col].char());
				sb.add(" ");
			}
			sb.add("\n");
		}
		return sb.toString();
	}
}

enum Cell
{
	Empty;
	Wall;
	SnakeHead;
	SnakeBody;
	Pellet;
}

class Cells extends Sprite
{
	static var strEmpty = ".";
	static var strWall = "#";
	static var strSnakeHead = "0";
	static var strSnakeBody = "=";
	static var strPellet = "+";
	
	static var colEmpty = 0xFFFFFF;
	static var colWall = 0x444444;
	static var colSnakeHead = 0xCCAAAA;
	static var colSnakeBody = 0xAAAACC;
	static var colPellet = 0x00FF44;
	
	public static function char(c : Cell) : String 
	{
		switch(c) 
		{
			case Cell.Empty: return strEmpty;
			case Cell.Wall: return strWall;
			case Cell.SnakeHead: return strSnakeHead;
			case Cell.SnakeBody: return strSnakeBody;
			case Cell.Pellet: return strPellet;
		}
	}
	
	public static function draw(c : Cell, cellSize : Float):Sprite
	{
		var col : UInt;
		switch (c) 
		{
			case Cell.Empty: col = colEmpty;
			case Cell.Wall: col = colWall;
			case Cell.SnakeHead: col = colSnakeHead;
			case Cell.SnakeBody: col = colSnakeBody;
			case Cell.Pellet: col = colPellet;	
		}
		return drawCell(col, cellSize);
	}
	
	private static function drawCell(color : UInt, cellSize : Float):Sprite
	{
		var sprite = new Sprite();
		sprite.graphics.beginFill(color);
		sprite.graphics.drawRect(0, 0, cellSize, cellSize);
		sprite.graphics.endFill();
		return sprite;
	}
}