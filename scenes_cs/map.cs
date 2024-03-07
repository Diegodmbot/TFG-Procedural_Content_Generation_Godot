using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class map : Node2D
{
	[Export] private Vector2 Borders { get; set; } = new Vector2(100, 100);
	readonly Vector2[] Directions = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];
	private enum MapType
	{
		AREA,
		WALLS,
		DOORS,
		GROUND
	}

	List<byte[,]> Structure { get; set; } = [];
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;


	public override void _Ready()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		GenerateRooms();
		GenerateBorders();
		SetDoors();
		DrawMap();
	}


	private void GenerateRooms()
	{
		var map = voronoiDiagram.BuildVoronoiDiagram(Borders);
		Structure[(int)MapType.AREA] = map;
	}

	private void GenerateBorders()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				if (i == 0 || i == Borders.X - 1 || j == 0 || j == Borders.Y - 1)
				{
					Structure[(int)MapType.WALLS][i, j] = 1;
				}
				else
				{
					foreach (var direction in Directions)
					{
						byte neighbor = Structure[(int)MapType.AREA][i + (int)direction.X, j + (int)direction.Y];
						if (Structure[(int)MapType.AREA][i, j] != neighbor && neighbor != 0)
						{
							// El muro guarda el valor de la habitación adyacente
							Structure[(int)MapType.WALLS][i, j] = neighbor;
							break;
						}
					}
				}
			}
		}
	}

	// En Structure[mapType.Doors] se guardan las puertas en la posicion correspondiente y la casilla de suelo desde la que se entra. 
	// Cada puerta tendrá un valor identico en la entrada y la salida.
	// Las puertas se generan en uno de los muros (A) de las habitaciones donde haya
	// una habitación adyacente (B) y otra habitación diferente (C) a 2 casillas de distancia en
	// la misma dirección y sentido contrario. Además se guarda el muro opuesto de la habitación vecina (D). A, B, C y D tendrán el mismo valor.
	private void SetDoors()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		int counter = 0;
		while (counter < 10)
		{
			int doorX = new Random().Next(1, (int)Borders.X - 1);
			int doorY = new Random().Next(1, (int)Borders.Y - 1);
			if (Structure[(int)MapType.WALLS][doorX, doorY] != 0)
			{
				foreach (var direction in Directions)
				{
					int adjacentWallX = doorX + (int)direction.X;
					int adjacentWallY = doorY + (int)direction.Y;
					// Si la casilla adyacente no es un muro y es de la misma habitación que la puerta
					if (Structure[(int)MapType.WALLS][adjacentWallX, adjacentWallY] == 0 && Structure[(int)MapType.AREA][adjacentWallX, adjacentWallY] == Structure[(int)MapType.AREA][doorX, doorY])
					{
						Structure[(int)MapType.DOORS][doorX, doorY] = (byte)counter;
						Structure[(int)MapType.DOORS][adjacentWallX, adjacentWallY] = (byte)counter;
						// Se guarda el muro opuesto de la habitación vecina
						Structure[(int)MapType.DOORS][doorX - (int)direction.X, doorY - (int)direction.Y] = (byte)counter;
						// Comprobar que la habitación no sea un muro y que esté dentro del mapa
						Structure[(int)MapType.DOORS][doorX - (int)direction.X * 2, doorY - (int)direction.Y * 2] = (byte)counter;
						counter++;
					}
				}
			}
		}
	}

	private void DrawMap()
	{
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				// Pintar las puertas
				if (Structure[(int)MapType.DOORS][i, j] != 0)
				{
					tileMap.SetCell(1, new Vector2I(i, j), 2, new Vector2I(0, 0));
				}
				// Pintar los muros
				if (Structure[(int)MapType.WALLS][i, j] != 0)
				{
					tileMap.SetCell(0, new Vector2I(i, j), 4, new Vector2I(0, 0));
				}
				// Pintar el area de las habitaciones
				else
				{
					var tile_coords = new Vector2I(Structure[(int)MapType.AREA][i, j], 0);
					tileMap.SetCell(0, new Vector2I(i, j), 4, tile_coords);
				}
			}
		}
	}
}
