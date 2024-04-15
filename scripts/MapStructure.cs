using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapStructure : Node2D
{
	private enum MapType
	{
		AREA = 0,
		WALLS = 1,
		DOORS = 2,
		GROUND = 3
	}

	private enum NeighborType
	{
		NEIGHBOORS = 1,
		COUNTED = 2,
		DOORS = 3
	}


	[Export] Godot.Vector2 ExportedBorders = new(100, 100);

	readonly System.Numerics.Vector2[] Directions = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];
	const double MinimumGroundPerRoom = 0.3;

	VoronoiDiagram VoronoiDiagram;
	TileMap DungeonTileMap;
	// map size
	private System.Numerics.Vector2 _borders;
	public List<byte[,]> Structure = [];
	// Si el número en la posición [i,j] es diferente a 0 significa que las habitaciones i y j están conectadas
	byte[,] Neighborhood;
	// Guarda la posición de las puertas de cada habitación
	List<System.Numerics.Vector2>[] DoorsPositions;
	List<System.Numerics.Vector2>[] SpawnPositions;
	(int area, int ground)[] RoomsSurface;


	public override void _Ready()
	{
		VoronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		DungeonTileMap = GetNode<TileMap>("DungeonTileMap");
		DungeonTileMap.Clear();
		int pointsCount = VoronoiDiagram.PointsLimit + 1;
		_borders = new(ExportedBorders.X, ExportedBorders.Y);
		// Lo sé... si lo cambio la escena no se carga. Simplemente ignoralo, NO LO MIRES!!!
		Structure.Add(new byte[(int)_borders.X, (int)_borders.Y]);
		Structure.Add(new byte[(int)_borders.X, (int)_borders.Y]);
		Structure.Add(new byte[(int)_borders.X, (int)_borders.Y]);
		Structure.Add(new byte[(int)_borders.X, (int)_borders.Y]);
		Neighborhood = new byte[pointsCount, pointsCount];
		DoorsPositions = Enumerable.Range(0, pointsCount)
														.Select(_ => new List<System.Numerics.Vector2>())
														.ToArray();
		SpawnPositions = Enumerable.Range(0, pointsCount)
														.Select(_ => new List<System.Numerics.Vector2>())
														.ToArray();
		RoomsSurface = new (int, int)[pointsCount];
	}

	public void GenerateMapStructure()
	{
		CreateRooms();
		GenerateBorders();
		SetNeighborsConnections();
		SetDoors();
	}

	public Array<Vector2> GetRoom(int roomId)
	{
		Array<Vector2> structure = [];
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				if (Structure[(int)MapType.GROUND][i, j] == roomId)
				{
					structure.Add(new Vector2(i, j));
				}
			}
		}
		return structure;
	}

	public Array<Array<int>> GetLayer(int layerId)
	{
		Array<Array<int>> layer = [];
		layer.Resize((int)_borders.X);
		for (int i = 0; i < layer.Count; i++)
		{
			layer[i] = [];
			layer[i].Resize((int)_borders.Y);
			for (int j = 0; j < layer[i].Count; j++)
			{
				layer[i][j] = Structure[layerId][i, j];
			}
		}
		return layer;
	}

	public Array<Vector2> GetDoors()
	{
		var doors = new Array<Vector2>();
		doors.Resize(DoorsPositions.SelectMany(d => d).Count());
		for (int i = 0; i < DoorsPositions.Length; i++)
		{
			for (int j = 0; j < DoorsPositions[i].Count; j++)
			{
				int doorId = Structure[(int)MapType.DOORS][(int)DoorsPositions[i][j].X, (int)DoorsPositions[i][j].Y];
				Vector2 doorPosition = new((int)DoorsPositions[i][j].X, (int)DoorsPositions[i][j].Y);
				doors[doorId] = doorPosition;
			}
		}
		return doors;
	}

	public Array<Vector2> GetSpawnsPositions()
	{
		Array<Vector2> spawns = [];
		spawns.Resize(SpawnPositions.SelectMany(d => d).Count());
		for (int i = 0; i < SpawnPositions.Length; i++)
		{
			for (int j = 0; j < SpawnPositions[i].Count; j++)
			{
				int id = Structure[(int)MapType.DOORS][(int)SpawnPositions[i][j].X, (int)SpawnPositions[i][j].Y];
				Vector2 position = new((int)SpawnPositions[i][j].X, (int)SpawnPositions[i][j].Y);
				spawns[id] = position;
			}
		}
		return spawns;
	}

	private int GetRoomByPosition(int coord_x, int coord_y)
	{
		return Structure[(int)MapType.AREA][coord_x, coord_y];
	}


	private void CreateRooms()
	{
		var map = VoronoiDiagram.BuildVoronoiDiagram(_borders);
		Structure[(int)MapType.AREA] = map;
	}

	private void GenerateBorders()
	{
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				if (i == 0 || i == _borders.X - 1 || j == 0 || j == _borders.Y - 1)
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
							Neighborhood[Structure[(int)MapType.AREA][i, j], neighbor] = (byte)NeighborType.NEIGHBOORS;
							break;
						}
					}
					if (Structure[(int)MapType.WALLS][i, j] == 0)
					{
						RoomsSurface[Structure[(int)MapType.AREA][i, j]].area += 1;
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
					Neighborhood[i, j] = (byte)NeighborType.COUNTED;
				}
			}
		}
	}

	private void SetDoors()
	{
		byte doorId = 0;
		bool allDoorsSet = false;
		Random random = new();
		while (allDoorsSet == false)
		{
			int doorX = random.Next(1, (int)_borders.X - 1);
			int doorY = random.Next(1, (int)_borders.Y - 1);
			// La casilla tiener que ser un muro
			if (Structure[(int)MapType.WALLS][doorX, doorY] != 0)
			{
				foreach (var direction in Directions)
				{
					int adjacentRoomX = doorX + (int)direction.X;
					int adjacentRoomY = doorY + (int)direction.Y;
					if (Structure[(int)MapType.WALLS][adjacentRoomX, adjacentRoomY] == 0)
					{
						int oppositeRoomX = doorX - (int)direction.X * 2;
						int oppositeRoomY = doorY - (int)direction.Y * 2;
						if (oppositeRoomX > 0 && oppositeRoomX < _borders.X && oppositeRoomY > 0 && oppositeRoomY < _borders.Y && Structure[(int)MapType.WALLS][oppositeRoomX, oppositeRoomY] == 0)
						{
							if (Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] == 2)
							{
								// Las puertas tienen un número par y los suelos de las habitaciones opuetas son impares
								Structure[(int)MapType.DOORS][doorX, doorY] = doorId;
								Structure[(int)MapType.DOORS][adjacentRoomX, adjacentRoomY] = doorId++;
								Structure[(int)MapType.DOORS][doorX - (int)direction.X, doorY - (int)direction.Y] = doorId;
								Structure[(int)MapType.DOORS][oppositeRoomX, oppositeRoomY] = doorId++;
								// Guardar la vencidad
								Neighborhood[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY], Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]] = (byte)NeighborType.DOORS;
								Neighborhood[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY], Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]] = (byte)NeighborType.DOORS;
								// Guardar la posicion de las puertas
								DoorsPositions[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]].Add(new System.Numerics.Vector2(doorX, doorY));
								DoorsPositions[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]].Add(new System.Numerics.Vector2(doorX - (int)direction.X, doorY - (int)direction.Y));
								// Guardar la posición del spawn
								SpawnPositions[Structure[(int)MapType.AREA][adjacentRoomX, adjacentRoomY]].Add(new System.Numerics.Vector2(adjacentRoomX, adjacentRoomY));
								SpawnPositions[Structure[(int)MapType.AREA][oppositeRoomX, oppositeRoomY]].Add(new System.Numerics.Vector2(oppositeRoomX, oppositeRoomY));
								break;
							}
						}
					}
				}
				for (int i = 0; i < Neighborhood.GetLength(0); i++)
				{
					for (int j = 0; j < Neighborhood.GetLength(1); j++)
					{
						if (Neighborhood[i, j] == (byte)NeighborType.COUNTED)
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

	private void GenerateGround(int roomId)
	{
		List<System.Numerics.Vector2> automatas;
		automatas = new(DoorsPositions[roomId]);
		bool roomCreated = false;
		while (!roomCreated)
		{
			roomCreated = (double)RoomsSurface[roomId].ground / RoomsSurface[roomId].area > MinimumGroundPerRoom && PathConnected(automatas[0], roomId);
			for (int j = 0; j < automatas.Count; j++)
			{
				if (Structure[(int)MapType.GROUND][(int)automatas[j].X, (int)automatas[j].Y] == 0)
				{
					RoomsSurface[roomId].ground += 1;
				}
				Structure[(int)MapType.GROUND][(int)automatas[j].X, (int)automatas[j].Y] = (byte)roomId;
				automatas[j] = MoveAutomata(automatas[j]);
			}
		}
	}

	private System.Numerics.Vector2 MoveAutomata(System.Numerics.Vector2 automata)
	{
		Random random = new();
		int randomNumber = random.Next(0, 4);
		System.Numerics.Vector2 targetPosition = automata + Directions[randomNumber];
		while (Structure[(int)MapType.WALLS][(int)targetPosition.X, (int)targetPosition.Y] != 0)
		{
			randomNumber = random.Next(0, 4);
			targetPosition = automata + Directions[randomNumber];
		}
		return targetPosition;
	}

	private bool PathConnected(System.Numerics.Vector2 initialPosition, int roomId)
	{
		Queue<System.Numerics.Vector2> queue = new();
		bool[,] visited = new bool[(int)_borders.X, (int)_borders.Y];
		queue.Enqueue(initialPosition);
		visited[(int)initialPosition.X, (int)initialPosition.Y] = true;
		List<System.Numerics.Vector2> doors = new(DoorsPositions[roomId]);
		while (queue.Count > 0)
		{
			System.Numerics.Vector2 current = queue.Dequeue();
			foreach (var direction in Directions)
			{
				System.Numerics.Vector2 neighbor = current + direction;
				if (neighbor.X >= 0 && neighbor.X < _borders.X && neighbor.Y >= 0 && neighbor.Y < _borders.Y)
				{
					if (Structure[(int)MapType.GROUND][(int)neighbor.X, (int)neighbor.Y] != 0 && !visited[(int)neighbor.X, (int)neighbor.Y])
					{
						queue.Enqueue(neighbor);
						visited[(int)neighbor.X, (int)neighbor.Y] = true;
						if (doors.Contains(neighbor))
						{
							doors.Remove(neighbor);
						}
					}
				}
			}
		}
		return doors.Count == 0;
	}

	private void DrawRoom(int roomId)
	{
		List<System.Numerics.Vector2> groundTiles = [];
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				if (Structure[(int)MapType.AREA][i, j] == roomId)
				{

					if (Structure[(int)MapType.GROUND][i, j] == 0)
					{
						DungeonTileMap.SetCell(0, new Vector2I(i, j), 2, Vector2I.Zero);
					}
					else
					{
						groundTiles.Add(new System.Numerics.Vector2(i, j));
					}
				}
			}
		}
		Array<Vector2I> groundTilesArray = new(groundTiles.Select(v => new Vector2I((int)v.X, (int)v.Y)).ToArray());
		DungeonTileMap.SetCellsTerrainConnect(0, groundTilesArray, 0, 0);
	}

	private void DrawLockedMap()
	{
		List<System.Numerics.Vector2> tiles = [];
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				tiles.Add(new System.Numerics.Vector2(i, j));
			}
		}
		Array<Vector2I> groundTilesArray = new(tiles.Select(v => new Vector2I((int)v.X, (int)v.Y)).ToArray());
		DungeonTileMap.SetCellsTerrainConnect(1, groundTilesArray, 0, 1);
	}


	private void DrawUnlockedRoom(int roomId)
	{
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				if (Structure[(int)MapType.AREA][i, j] == roomId)
				{
					DungeonTileMap.SetCell(1, new Vector2I(i, j));
				}
			}
		}
		DungeonTileMap.UpdateInternals();
	}
}

