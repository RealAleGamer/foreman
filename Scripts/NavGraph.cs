using Godot;
using System;
using System.Linq;

public partial class NavGraph : Node2D
{
	[Export]
	public TileMap tileMap;

	[Export]
	public Rect2I bounds;

	private int tileSize;
	private int halfTile;
	private AStar2D pathing;
	private long pointsEnd;

	public override void _Ready()
	{
		pathing = new AStar2D();

		tileSize = tileMap.TileSet.TileSize.X;
		halfTile = tileSize / 2;

		var xMax = (int)(bounds.Position.X + bounds.Size.X)/tileSize;
		var xMin = (int)bounds.Position.X/tileSize;
		var yMin = (int)bounds.Position.Y/tileSize;
		var yMax = (int)(bounds.Position.Y + bounds.Size.Y)/tileSize;

		for (int x = xMin; x <= xMax; ++x)
		{
			for (int y = yMin; y <= yMax; ++y)
			{
				if (IsGroundPoint(x, y))
				{
					pathing.AddPoint(GetId(x, y), new Vector2(x * tileSize + halfTile, y * tileSize + halfTile));
				}
			}
		}

		var ids = pathing.GetPointIds();
		foreach (var id in ids)
		{
			var coord = GetCoordFromId(id);
			
			for (int x = -1; x <= 1; ++x)
			{
				for (int y = -1; y <= 1; ++y)
				{
					if (x == 0 && y == 0) continue;

					var neighborId = GetId(coord.X + x, coord.Y + y);
					if (ids.Contains(neighborId))
					{
						pathing.ConnectPoints(id, neighborId);
					}
				}
			}
		}
	}

	public Vector2[] GetPath(Vector2 start, Vector2 end)
	{
		return pathing.GetPointPath(pathing.GetClosestPoint(start), pathing.GetClosestPoint(end));
	}

/*     public override void _Draw()
    {
		foreach (var id in pathing.GetPointIds())
		{
			var pos = pathing.GetPointPosition(id);

			var space = tileMap.GetCellTileData(3, new Vector2I((int)pos.X / tileSize, (int)pos.Y / tileSize), false);
			if (space == null)
			{
				var ground = tileMap.GetCellTileData(3, new Vector2I((int)pos.X / tileSize, 1 + (int)pos.Y / tileSize), false);
				if (ground == null)
				{
					DrawCircle(pos, 2, new Color(0, 1, 1));
				}
				else
				{
					DrawCircle(pos, 2, new Color(0, 1, 0));
				}
			}
			else
			{
				DrawCircle(pos, 2, new Color(1, 0, 0));
			}

			foreach (var connection in pathing.GetPointConnections(id))
			{
				var endPos = pathing.GetPointPosition(connection);
				DrawLine(pos, endPos, new Color(1,0,0));
			}
		} 
    }
	 */
	private bool IsGroundPoint(int x, int y)
	{
		var space = tileMap.GetCellTileData(3, new Vector2I(x, y), false);
		if (space == null)
		{
			var ground = tileMap.GetCellTileData(3, new Vector2I(x, 1 + y), false);
			return ground != null;
		}
		return false;
	}
	
	private long GetId(int x, int y)
	{
		return (long)x * bounds.Size.Y + y;
		/*
		
			0,0 1,0 2,0 3,0 4,0 
			0,1 1,1 2,1 3,1 4,1
			0,2 1,2 2,2 3,2 4,2
			0,3 1,3 2,3 3,3 4,3
			0,4 1,4 2,4 3,4 4,4	
		
			0   5   10  15  20 
			1   6   11  16  21
			2   7   12  17  22
			3   8   13  18  23
			4   9   14  19  24
		
		*/
	}

	private Vector2I GetCoordFromId(long id)
	{
		var y = id % bounds.Size.Y;
		var x = id / bounds.Size.X;
		return new Vector2I((int)x, (int)y);
	}

	private Vector2 GetPosFromId(long id)
	{
		var coord = GetCoordFromId(id);
		return new Vector2I(coord.X * tileSize + halfTile, coord.Y * tileSize + halfTile);
	}
}
