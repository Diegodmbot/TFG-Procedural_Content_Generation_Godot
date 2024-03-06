using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class map : Node2D
{
	[Export] private Vector2 Borders { get; set; } = new Vector2(100, 100);

	readonly Vector2[] Directions = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];

	List<int[,]> Structure { get; set; } = [];
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;


	public override void _Ready()
	{
		Structure.Add(new int[(int)Borders.X, (int)Borders.Y]);
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		GenerateRooms();
		GenerateBorders();
		SetDoors();
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

	private void SetDoors()
	{
		// En Structure[1] se guardan las puertas en la posicion correspondiente. 
		// Si hay una puerta, el valor es 1, si no, es 0.
		// Las puertas se generan en uno de los muros de las habitaciones donde haya
		// una habitación adyacente y otra habitación adyacente a 2 casillas de distancia en
		// la misma dirección y sentido contrario.
		Structure.Add(new int[(int)Borders.X, (int)Borders.Y]);
		for (int i = 2; i < Borders.X - 2; i++)
		{
			for (int j = 2; j < Borders.Y - 2; j++)
			{
				if (Structure[0][i, j] == 0)
				{
					foreach (var direction in Directions)
					{
						int neighbor = Structure[0][i + (int)direction.X, j + (int)direction.Y];
						int neighbor2 = Structure[0][i + (int)direction.X * -2, j + (int)direction.Y * -2];
						if (neighbor != 0 && neighbor2 != 0 && neighbor != neighbor2)
						{
							Structure[1][i, j] = 1;
							Structure[1][i + (int)direction.X, j + (int)direction.Y] = 1;
							Structure[1][i + (int)direction.X * -2, j + (int)direction.Y * -2] = 1;
							// Structure[1][i + (int)direction.X, j + (int)direction.Y] = 1;
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
				var tile_coords = new Vector2I(Structure[0][i, j], 0);
				tileMap.SetCell(0, new Vector2I(i, j), 4, tile_coords);
				if (Structure[1][i, j] == 1)
				{
					tileMap.SetCell(1, new Vector2I(i, j), 2, new Vector2I(0, 0));
				}
			}
		}
	}
}
