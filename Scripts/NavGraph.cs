using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public enum PointType
{
	Climbable,
	Walkable,
}
public partial class NavGraph : Node2D
{
	[Export]
	public TileMapLayer tileMapGround;

	[Export]
	public TileMapLayer tileMapClimbable;

	[Export]
	public Rect2I bounds;

	[Export]
	public bool drawDebug = false;

	private int tileSize;
	private int halfTile;
	private AStar2D pathing;
	private long pointsEnd;

	public override void _Ready()
	{
		pathing = new AStar2D();

		tileSize = tileMapGround.TileSet.TileSize.X;
		halfTile = tileSize / 2;

		var xMax = (int)(bounds.Position.X + bounds.Size.X) / tileSize;
		var xMin = (int)bounds.Position.X / tileSize;
		var yMin = (int)bounds.Position.Y / tileSize;
		var yMax = (int)(bounds.Position.Y + bounds.Size.Y) / tileSize;

		var types = new Dictionary<long, PointType>();

		for (int x = xMin; x <= xMax; ++x)
		{
			for (int y = yMin; y <= yMax; ++y)
			{
				if (IsGroundPoint(x, y))
				{
					var id = GetId(x, y);
					pathing.AddPoint(id, new Vector2(x * tileSize + halfTile, y * tileSize + halfTile));
					types.Add(id, PointType.Walkable);
				}
				else if (IsClimbable(x, y))
				{
					var id = GetId(x, y);
					pathing.AddPoint(id, new Vector2(x * tileSize + halfTile, y * tileSize + halfTile));
					types.Add(id, PointType.Climbable);
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
					// Don't try and link this point with itself
					if (x == 0 && y == 0) continue;

					var neighborId = GetId(coord.X + x, coord.Y + y);
					if (ids.Contains(neighborId))
					{
						if (types[id] == PointType.Walkable && types[neighborId] == PointType.Walkable)
						{
							pathing.ConnectPoints(id, neighborId);
						}
						else
						{
							if (x != 0) continue;
							pathing.ConnectPoints(id, neighborId);
						}
					}
				}
			}
		}
	}

	public Vector2[] GetPath(Vector2 start, Vector2 end)
	{
		return pathing.GetPointPath(pathing.GetClosestPoint(start), pathing.GetClosestPoint(end));
	}

	public Vector2 FindNearestPosition(Vector2 pos)
	{
		return new Vector2((int)pos.X, (int)pos.Y);
	}

	public override void _Draw()
	{
		if (!drawDebug)
		{
			return;
		}

		foreach (var id in pathing.GetPointIds())
		{
			var pos = pathing.GetPointPosition(id);

			var space = tileMapGround.GetCellTileData(new Vector2I((int)pos.X / tileSize, (int)pos.Y / tileSize));
			if (space == null)
			{
				var ground = tileMapGround.GetCellTileData(new Vector2I((int)pos.X / tileSize, 1 + (int)pos.Y / tileSize));
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
				DrawLine(pos, endPos, new Color(1, 0, 0));
			}
		}
	}

	private bool IsGroundPoint(int x, int y)
	{
		var space = tileMapGround.GetCellTileData(new Vector2I(x, y));
		if (space == null)
		{
			var ground = tileMapGround.GetCellTileData(new Vector2I(x, 1 + y));
			return ground != null;
		}
		return false;
	}

	private bool IsClimbable(int x, int y)
	{
		var lader = tileMapClimbable.GetCellTileData(new Vector2I(x, y));
		return lader != null;
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
