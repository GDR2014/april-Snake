package ;

using SnakeWorld;

class Snake implements IWorldObject
{

	var world : SnakeWorld;
	public var direction : Direction;
	
	var head : Head;
	var tail : Segment;
	
	var shouldGrow : Bool;
	
	public var supposedSize : Int;
	var trueSize : Int;
	
	public function new(world : SnakeWorld, ?x : Int, ?y : Int, ?dir : Direction) 
	{
		this.world = world;
		
		if ( dir == null ) dir = Right;
		if ( x == null ) {
			if (dir == Up || dir == Down)
				x = Std.int(.5 * world.width);
			else if ( dir == Left )
				x = Std.int(.75 * world.width);
			else if ( dir == Right )
				x = Std.int(.25 * world.width);
		}
		if ( y == null ) {
			if (dir == Left || dir == Right)
				y = Std.int(.5 * world.height);
			else if ( dir == Up )
				y = Std.int(.25 * world.width);
			else if ( dir == Down )
				y = Std.int(.75 * world.height);
		}
		
		this.head = new Head(x, y);
		this.tail = head;
		this.supposedSize = 1;
		this.trueSize = 1;
		this.direction = dir;
	}
	
	public function move() : Void
	{
		var seg : Segment = head;
		
		var oldX = head.x;
		var oldY = head.y;
		var newX = head.x;
		var newY = head.y;
		if ( direction == Left ) newX--;
		if ( direction == Right ) newX++;
		if ( direction == Up ) newY--;
		if ( direction == Down ) newY++;
		
		while ( seg != null ) {
			oldX = seg.x;
			oldY = seg.y;
			seg.x = newX;
			seg.y = newY;
			newX = oldX;
			newY = oldY;
			seg = seg.child;
		}
		if (trueSize < supposedSize) grow(oldX, oldY);
	}
	
	public function turn(dir : Direction) : Void 
	{
		switch (dir) 
		{
			case Up:
				if ( direction != Down ) direction = Up;
			case Down:
				if ( direction != Up ) direction = Down;
			case Left:
				if ( direction != Right ) direction = Left;
			case Right:
				if ( direction != Left ) direction = Right;
		}
	}
	
	function grow(x : Int, y : Int) : Void 
	{
		trace("Growing..");
		var segment = new Segment(tail, x, y);
		tail = segment;
		trueSize++;
	}
	
	public function reportPosition(grid : Array<Array<SnakeWorld.Cell>>)
	{
		for ( seg in this )
			seg.reportPosition(grid);
	}

	// Iterator stuff
	public function iterator()
	{
		return new SnakeIterator(head);
	}
	
}

	
enum Direction
{
	Left;
	Right;
	Up;
	Down;
}
	
class Segment implements IWorldObject
{
	public var x : Int;
	public var y : Int;
	
	public var parent(default, set) : Segment;
	public var child : Segment;
	
	var type = SnakeBody;
	
	public function new(parent : Segment, x : Int, y : Int):Void 
	{
		if( parent != null )
			this.parent = parent;
		this.x = x;
		this.y = y;
		trace('Created segment: x=$x, y=$y, parent=$parent');
	}
	
	public function reportPosition(grid : Array<Array<Cell>>)
	{
		trace("Reporting!");
		grid[y][x] = type;
	}
	
	function set_parent(value:Segment):Segment 
	{
		value.child = this;
		return parent = value;
	}
}

class Head extends Segment
{
	
	public function new(x : Int, y : Int):Void 
	{
		super(null, x, y);
		type = SnakeHead;
	}
}

class SnakeIterator {
	var current : Segment;
	
	public function new(head : Head)
	{
		current = new Segment(null, 0, 0);
		current.child = head;
	}
	
	public function hasNext()
	{
		var has = current.child != null;
		return has;
	}
	
	public function next() 
	{
		current = current.child;
		return current;
	}
}