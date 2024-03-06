using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class map : Node2D
{
	[Export] private Vector2 Borders { get; set; } = new Vector2(100, 100);

	readonly Vector2[] Directions = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];

	// int[,,] Structure { get; set; }
	List<int[,]> Structure { get; set; } = [];
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;


	public override void _Ready()
	{
		// Structure = new int[(int)Borders.X, (int)Borders.Y, 3];
		Structure.Add(new int[(int)Borders.X, (int)Borders.Y]);
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		GenerateRooms();
		GenerateBorders();
		drawMap();
	}


	private void GenerateRooms()
	{
		var map = voronoiDiagram.BuildVoronoiDiagram(Borders);
		Structure[0] = map;
	}

	private void GenerateBorders()
	{
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				if (i == 0 || i == Borders.X - 1 || j == 0 || j == Borders.Y - 1)
				{
					Structure[0][i, j] = 0;
				}
				else
				{
					foreach (var direction in Directions)
					{
						int neighbor = Structure[0][i + (int)direction.X, j + (int)direction.Y];
						if (Structure[0][i, j] != neighbor && neighbor != 0)
						{
							Structure[0][i, j] = 0;
							break;
						}
					}
				}
			}
		}
	}

	private void drawMap()
	{
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				// El valor de la celda en el mapa de Voronoi es el id del punto mas cercano mas 1, porque las celdas en el tilemap que empiezan en 0 son vacias 
				var tile_coords = new Vector2I(Structure[0][i, j], 0);
				tileMap.SetCell(0, new Vector2I(i, j), 4, tile_coords);
			}
		}
	}
}
