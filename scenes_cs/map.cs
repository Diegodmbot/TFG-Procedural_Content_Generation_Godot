using Godot;
using Godot.Collections;
using System;

public partial class map : Node2D
{
	[Export] private Vector2 Borders { get; set; } = new Vector2(100, 100);

	int[,,] Structure { get; set; }
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;


	public override void _Ready()
	{
		Structure = new int[(int)Borders.X, (int)Borders.Y, 3];
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		GenerateRooms();
		GenerateBorders();
		drawMap();
	}


	private void GenerateRooms()
	{
		var map = voronoiDiagram.BuildVoronoiDiagram(Borders);
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				Structure[i, j, 0] = map[i, j];
			}
		}
	}

	private void GenerateBorders()
	{
		throw new NotImplementedException();
	}

	private void drawMap()
	{
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				var tile_coords = new Vector2I(Structure[i, j, 0], 3);
				tileMap.SetCell(0, new Vector2I(i, j), 1, tile_coords);
			}
		}
	}
}
