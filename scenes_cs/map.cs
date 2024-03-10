using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

// struct Room
// {
// 	// Guarda en el indice i el id de la habitación adyacente a la habitación i
// 	public List<byte> Neighboors { get; set; }
// 	public List<byte> Doors { get; set; }

// 	public Room()
// 	{
// 		Neighboors = [];
// 		Doors = [];
// 	}
// }

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
	byte[,] Neighborhood;
	VoronoiDiagram voronoiDiagram;
	TileMap tileMap;


	public override void _Ready()
	{
		voronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		tileMap = GetNode<TileMap>("TileMap");
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		Neighborhood = new byte[voronoiDiagram.PointsLimit + 1, voronoiDiagram.PointsLimit + 1];
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
							// El muro guarda el id de la habitación adyacente
							Structure[(int)MapType.WALLS][i, j] = neighbor;
							Neighborhood[Structure[(int)MapType.AREA][i, j], neighbor] = 1;
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
		bool allDoorsSet = false;
		int neighborhoodCount = 0;
		for (int i = 0; i < Neighborhood.GetLength(0); i++)
		{
			for (int j = 0; j < Neighborhood.GetLength(1); j++)
			{
				if (Neighborhood[i, j] == 1)
				{
					Neighborhood[i, j] = 2;
					Neighborhood[j, i] = 2;
					neighborhoodCount++;
				}
			}
		}
		Random random = new();
		while (allDoorsSet == false)
		{
			int doorX = random.Next(1, (int)Borders.X - 1);
			int doorY = random.Next(1, (int)Borders.Y - 1);
			// La casilla tiener que ser un muro
			if (Structure[(int)MapType.WALLS][doorX, doorY] != 0)
			{
				foreach (var direction in Directions)
				{
					int adjacentRoomX = doorX + (int)direction.X;
					int adjacentRoomY = doorY + (int)direction.Y;
					// La casilla adyacente no puede ser un muro y ser de la misma habitación que la puerta
					if (Structure[(int)MapType.WALLS][adjacentRoomX, adjacentRoomY] == 0)
					{
						int oppositeRoomX = doorX - (int)direction.X * 2;
						int oppositeRoomY = doorY - (int)direction.Y * 2;
						// La casilla de la habitación opuesta no pueder ser un muro
						if (oppositeRoomX > 0 && oppositeRoomX < Borders.X && oppositeRoomY > 0 && oppositeRoomY < Borders.Y && Structure[(int)MapType.WALLS][oppositeRoomX, oppositeRoomY] == 0)
						{
							if (Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] == 2)
							{
								byte doorId = (byte)counter;
								Structure[(int)MapType.DOORS][doorX, doorY] = doorId;
								Structure[(int)MapType.DOORS][adjacentRoomX, adjacentRoomY] = doorId;
								// Se guarda el muro opuesto de la habitación vecina
								Structure[(int)MapType.DOORS][doorX - (int)direction.X, doorY - (int)direction.Y] = doorId;
								Structure[(int)MapType.DOORS][oppositeRoomX, oppositeRoomY] = doorId;
								Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] = 3;
								Neighborhood[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY], Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]] = 3;
								counter++;
								break;
							}
						}
					}
				}
				if (counter == neighborhoodCount)
				{
					allDoorsSet = true;
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
					var tile_coords = new Vector2I(Structure[(int)MapType.AREA][i, j] + 1, 0);
					tileMap.SetCell(0, new Vector2I(i, j), 4, tile_coords);
				}
			}
		}
	}
}
