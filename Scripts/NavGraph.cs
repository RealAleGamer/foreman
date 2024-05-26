using Godot;
using System;

public partial class NavGraph : Node2D
{
	[Export]
	public TileMap tileMap;

	[Export]
	public Rect2 bounds;

	private int tileSize;
	private AStar2D pathing;
	private long pointsEnd;

	public override void _Ready()
	{
		pathing = new AStar2D();

		tileSize = tileMap.TileSet.TileSize.X;
		var halfTile = tileSize / 2;

		var xMax = (int)(bounds.Position.X + bounds.Size.X)/tileSize;
		var xMin = (int)bounds.Position.X/tileSize;
		var yMin = (int)bounds.Position.Y/tileSize;
		var yMax = (int)(bounds.Position.Y + bounds.Size.Y)/tileSize;

		long id = 0;
		for (int x = xMin; x <= xMax; ++x)
		{
			for (int y = yMin; y <= yMax; ++y)
			{
				pathing.AddPoint(id, new Vector2(x * tileSize + halfTile, y * tileSize + halfTile));
				//GD.Print($"Added Point at: {x}, {y}");
				++id;
			}
		}
		pointsEnd = id;
	}

    public override void _Draw()
    {
		for (int i = 0; i < pointsEnd; ++i)
		{
			var pos = pathing.GetPointPosition(i);

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
		} 
    }
}
