using Godot;
using Godot.Collections;
using System;

public partial class map : Node2D
{
	int[,,] structure {get; set;}
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;
	[Export] private Vector2 borders { get; set; } = new Vector2(100, 100);

	
	public override void _Ready()
	{
		structure = new int[(int)borders.X, (int)borders.Y, 3];
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		GenerateRooms();
		drawMap();
	}

	private void GenerateRooms(){
		var map = voronoiDiagram.BuildVoronoiDiagram(borders);
		foreach (Dictionary point in map)
		{
			foreach (Vector2 citizen in (Array<Vector2>)point["citizens"])
			{
				structure[(int)citizen.X, (int)citizen.Y, 0] = (int)point["id"];
			}
		}
	}

	private void drawMap(){
		for (int i = 0; i < borders.X; i++) {
			for (int j = 0; j < borders.Y; j++) {
				var tile_coords = new Vector2I(structure[i,j,0],3);
				tileMap.SetCell(0, new Vector2I(i, j), 1, tile_coords);
			}
		}
	}
}
