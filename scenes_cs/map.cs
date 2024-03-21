using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

public partial class Map : Node2D
{
	[Export] Godot.Vector2 ExportedBorders = new(100, 100);

	private enum MapType
	{
		AREA = 0,
		WALLS = 1,
		DOORS = 2,
		GROUND = 3
	}

	private enum NeighboorType
	{
		NEIGHBOORS = 1,
		COUNTED = 2,
		DOORS = 3
	}


	readonly System.Numerics.Vector2[] Directions = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];

	private System.Numerics.Vector2 Borders;
	List<byte[,]> Structure { get; set; } = [];
	// Guarda las habitaciones conectadas los índices de los array representa el id de las habitaciones
	// Si el número en la posición [i,j] es diferente a 0 significa que las habitaciones i y j están conectadas
	byte[,] Neighborhood;
	VoronoiDiagram VoronoiDiagram;
	TileMap TileMap;
	// Guarda la posición de las puertas de cada habitación
	List<System.Numerics.Vector2>[] DoorsPositions;
	(int area, int ground)[] surfaces;


	public override void _Ready()
	{
		// Guardar nodos hijos
		VoronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		TileMap = GetNode<TileMap>("TileMap");
		// Número de puntos en el mapa
		int pointsCount = VoronoiDiagram.PointsLimit + 1;
		Borders = new(ExportedBorders.X, ExportedBorders.Y);
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		Neighborhood = new byte[pointsCount, pointsCount];
		DoorsPositions = Enumerable.Range(0, pointsCount)
														.Select(_ => new List<System.Numerics.Vector2>())
														.ToArray();
		surfaces = new (int, int)[pointsCount];
		// Generar mapa
		GenerateRooms();
		GenerateBorders();
		SetNeighborsConnections();
		SetDoors();
		RunRandomWalker();
		DrawMap();
		int id = 0;
		foreach (var room in surfaces)
		{
			GD.Print($"Room: {id++}, Area: {room.area}, Ground: {room.ground}, Diff: {(float)room.ground / room.area};");
		}
	}


	private void GenerateRooms()
	{
		var map = VoronoiDiagram.BuildVoronoiDiagram(Borders);
		Structure[(int)MapType.AREA] = map;
	}

	private void GenerateBorders()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		for (int i = 0; i < Borders.X; i++)
		{
			for (int j = 0; j < Borders.Y; j++)
			{
				// Si es un límete del mapa
				if (i == 0 || i == Borders.X - 1 || j == 0 || j == Borders.Y - 1)
				{
					Structure[(int)MapType.WALLS][i, j] = 1;
				}
				else
				{
					foreach (var direction in Directions)
					{
						byte neighbor = Structure[(int)MapType.AREA][i + (int)direction.X, j + (int)direction.Y];
						if (neighbor != Structure[(int)MapType.AREA][i, j] && neighbor != 0)
						{
							// El muro guarda el id de la habitación adyacente
							Structure[(int)MapType.WALLS][i, j] = neighbor;
							Neighborhood[Structure[(int)MapType.AREA][i, j], neighbor] = (byte)NeighboorType.NEIGHBOORS;
							break;
						}
					}
					if (Structure[(int)MapType.WALLS][i, j] == 0)
					{
						surfaces[Structure[(int)MapType.AREA][i, j]].area += 1;
					}
				}
			}
		}
	}

	private void SetNeighborsConnections()
	{
		for (int i = 1; i < Neighborhood.GetLength(0); i++)
		{
			for (int j = 1; j < i; j++)
			{
				if (Neighborhood[i, j] == 1)
				{
					Neighborhood[i, j] = (byte)NeighboorType.COUNTED;
				}
			}
		}
	}

	// En Structure[mapType.Doors] se guardan las puertas en la posicion correspondiente y la casilla de suelo desde la que se entra. 
	// Las puertas se generan en uno de los muros (A) de las habitaciones donde haya
	// una habitación adyacente (B) y otra habitación diferente (C) a 2 casillas de distancia en
	// la misma dirección y sentido contrario. Además se guarda el muro opuesto de la habitación vecina (D).
	// Cada puerta (A y D) tendrá un valor identico y único. Las entradas (B y C) tendrá el identificador de la habitación.
	private void SetDoors()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		byte counter = (byte)(VoronoiDiagram.PointsLimit + 1);
		bool allDoorsSet = false;
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
					// La casilla adyacente no puede ser un muro y tiene que ser de la misma habitación que la puerta
					if (Structure[(int)MapType.WALLS][adjacentRoomX, adjacentRoomY] == 0)
					{
						int oppositeRoomX = doorX - (int)direction.X * 2;
						int oppositeRoomY = doorY - (int)direction.Y * 2;
						// La casilla de la habitación opuesta no pueder ser un muro
						if (oppositeRoomX > 0 && oppositeRoomX < Borders.X && oppositeRoomY > 0 && oppositeRoomY < Borders.Y && Structure[(int)MapType.WALLS][oppositeRoomX, oppositeRoomY] == 0)
						{
							if (Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] == 2)
							{
								Structure[(int)MapType.DOORS][doorX, doorY] = counter;
								// Se guarda el muro opuesto de la habitación vecina
								Structure[(int)MapType.DOORS][doorX - (int)direction.X, doorY - (int)direction.Y] = counter;
								Structure[(int)MapType.DOORS][adjacentRoomX, adjacentRoomY] = counter;
								Structure[(int)MapType.DOORS][oppositeRoomX, oppositeRoomY] = counter;
								// Guardar la vencidad
								Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] = (byte)NeighboorType.DOORS;
								Neighborhood[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY], Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]] = (byte)NeighboorType.DOORS;
								// Guardar la posicion de las entradas
								DoorsPositions[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]].Add(new System.Numerics.Vector2(adjacentRoomX, adjacentRoomY));
								DoorsPositions[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]].Add(new System.Numerics.Vector2(oppositeRoomX, oppositeRoomY));
								counter++;
								break;
							}
						}
					}
				}
				for (int i = 0; i < Neighborhood.GetLength(0); i++)
				{
					for (int j = 0; j < Neighborhood.GetLength(1); j++)
					{
						if (Neighborhood[i, j] == (byte)NeighboorType.COUNTED)
						{
							allDoorsSet = false;
							break;
						}
						else
						{
							allDoorsSet = true;
						}
					}
				}
			}
		}
	}

	private void RunRandomWalker()
	{
		Structure.Add(new byte[(int)Borders.X, (int)Borders.Y]);
		Random random = new();
		System.Numerics.Vector2 targetPosition;
		System.Numerics.Vector2 automata;
		List<System.Numerics.Vector2> doors;
		for (int i = 1; i < DoorsPositions.Length; i++)
		{
			bool roomCreated = false;
			automata = new(DoorsPositions[i][0].X, DoorsPositions[i][0].Y);
			doors = new(DoorsPositions[i]);
			while (!roomCreated)
			{
				if (doors.Contains(automata))
				{
					doors.Remove(automata);
				}
				if (Structure[(int)MapType.GROUND][(int)automata.X, (int)automata.Y] == 0)
				{
					surfaces[i].ground += 1;
				}
				Structure[(int)MapType.GROUND][(int)automata.X, (int)automata.Y] = 1;
				int randomNumber = random.Next(0, 4);
				targetPosition = automata + Directions[randomNumber];
				while (Structure[(int)MapType.WALLS][(int)targetPosition.X, (int)targetPosition.Y] != 0)
				{
					randomNumber = random.Next(0, 4);
					targetPosition = automata + Directions[randomNumber];
				}
				automata = targetPosition;
				if (doors.Count == 0 && (double)surfaces[i].ground / surfaces[i].area > 0.30)
				{
					roomCreated = true;
				}
			}
			if ((float)surfaces[i].ground / surfaces[i].area > 0.70)
			{
				int counter = (int)(surfaces[i].ground - (surfaces[i].area * 0.70));
				for (int j = 0; j < counter; j++)
				{
					// eliminar puntos aleatorios de ground
					System.Numerics.Vector2 possiblePoint = new(random.Next((int)Borders.X), random.Next((int)Borders.Y));
					if (Structure[(int)MapType.AREA][(int)possiblePoint.X, (int)possiblePoint.Y] == i && Structure[(int)MapType.GROUND][(int)possiblePoint.X, (int)possiblePoint.Y] == 1)
					{
						Structure[(int)MapType.GROUND][(int)possiblePoint.X, (int)possiblePoint.Y] = 0;
						surfaces[i].ground -= 1;
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
					TileMap.SetCell(1, new Vector2I(i, j), 2, new Vector2I(0, 0));
				}
				if (Structure[(int)MapType.GROUND][i, j] != 0)
				{
					var tile_coords = new Vector2I(Structure[(int)MapType.AREA][i, j], 0);
					TileMap.SetCell(0, new Vector2I(i, j), 4, tile_coords);
				}
				else
				{
					TileMap.SetCell(0, new Vector2I(i, j), 4, new Vector2I(0, 0));
				}
			}
		}
	}
}
