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

	[Export] Vector2 ExportedBorders = new(100, 100);
	[Export] int RoomsCount = 10;
	readonly System.Numerics.Vector2[] Directions = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];
	const double MinimumGroundPerRoom = 0.3;

	VoronoiDiagram VoronoiDiagram;
	// Map size
	private System.Numerics.Vector2 _borders;
	// Store map structure in different layers
	public byte[,,] Structure;
	// Store rooms neighbors
	byte[,] Neighborhood;
	// Store the position of the doors of each room
	List<System.Numerics.Vector2>[] DoorsPositions;
	List<System.Numerics.Vector2>[] SpawnPositions;
	(int area, int ground)[] RoomsSurface;



	public override void _Ready()
	{
		VoronoiDiagram = GetNode<VoronoiDiagram>("VoronoiDiagram");
		_borders = new(ExportedBorders.X, ExportedBorders.Y);
		Structure = new byte[(int)_borders.X, (int)_borders.Y, 4];
		int roomsCountExtra = RoomsCount + 1;
		Neighborhood = new byte[roomsCountExtra + 1, roomsCountExtra + 1];
		DoorsPositions = Enumerable.Range(0, roomsCountExtra).Select(_ => new List<System.Numerics.Vector2>()).ToArray();
		SpawnPositions = Enumerable.Range(0, roomsCountExtra).Select(_ => new List<System.Numerics.Vector2>()).ToArray();
		RoomsSurface = new (int, int)[roomsCountExtra];
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
				if (Structure[i, j, (int)MapType.GROUND] == roomId && Structure[i, j, (int)MapType.DOORS] == 0)
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
				layer[i][j] = Structure[i, j, layerId];
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
				int doorId = Structure[(int)DoorsPositions[i][j].X, (int)DoorsPositions[i][j].Y, (int)MapType.DOORS];
				Vector2 doorPosition = new((int)DoorsPositions[i][j].X, (int)DoorsPositions[i][j].Y);
				doors[doorId - 1] = doorPosition;
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
				int id = Structure[(int)SpawnPositions[i][j].X, (int)SpawnPositions[i][j].Y, (int)MapType.DOORS];
				Vector2 position = new((int)SpawnPositions[i][j].X, (int)SpawnPositions[i][j].Y);
				spawns[id - 1] = position;
			}
		}
		return spawns;
	}

	private int GetRoomByPosition(int coord_x, int coord_y)
	{
		return Structure[coord_x, coord_y, (int)MapType.AREA];
	}


	private void CreateRooms()
	{
		var map = VoronoiDiagram.BuildVoronoiDiagram(_borders, RoomsCount);
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				Structure[i, j, (int)MapType.AREA] = map[i, j];
			}
		}
	}

	private void GenerateBorders()
	{
		for (int i = 0; i < _borders.X; i++)
		{
			for (int j = 0; j < _borders.Y; j++)
			{
				if (i == 0 || i == _borders.X - 1 || j == 0 || j == _borders.Y - 1)
				{
					Structure[i, j, (int)MapType.WALLS] = (byte)(RoomsCount + 1);
				}
				else
				{
					foreach (var direction in Directions)
					{
						byte neighbor = Structure[i + (int)direction.X, j + (int)direction.Y, (int)MapType.AREA];
						if (neighbor != Structure[i, j, (int)MapType.AREA] && neighbor != 0)
						{
							// El muro guarda el id de la habitación adyacente
							Structure[i, j, (int)MapType.WALLS] = neighbor;
							Neighborhood[Structure[i, j, (int)MapType.AREA], neighbor] = (byte)NeighborType.NEIGHBOORS;
							break;
						}
					}
					if (Structure[i, j, (int)MapType.WALLS] == 0)
					{
						RoomsSurface[Structure[i, j, (int)MapType.AREA]].area += 1;
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
				if (Neighborhood[i, j] == (byte)NeighborType.NEIGHBOORS)
				{
					Neighborhood[i, j] = (byte)NeighborType.COUNTED;
					Neighborhood[j, i] = (byte)NeighborType.COUNTED;
				}
			}
		}
	}

	private void SetDoors()
	{
		byte doorId = 1;
		bool allDoorsSet = false;
		Random random = new();
		while (allDoorsSet == false)
		{
			int doorX = random.Next(1, (int)_borders.X - 1);
			int doorY = random.Next(1, (int)_borders.Y - 1);
			// La casilla tiener que ser un muro
			if (Structure[doorX, doorY, (int)MapType.WALLS] != 0)
			{
				foreach (var direction in Directions)
				{
					int adjacentRoomX = doorX + (int)direction.X;
					int adjacentRoomY = doorY + (int)direction.Y;
					if (Structure[adjacentRoomX, adjacentRoomY, (int)MapType.WALLS] == 0)
					{
						int oppositeRoomX = doorX - (int)direction.X * 2;
						int oppositeRoomY = doorY - (int)direction.Y * 2;
						if (oppositeRoomX > 0 && oppositeRoomX < _borders.X && oppositeRoomY > 0 && oppositeRoomY < _borders.Y && Structure[oppositeRoomX, oppositeRoomY, (int)MapType.WALLS] == 0)
						{
							if (Neighborhood[Structure[adjacentRoomX, adjacentRoomY, (int)MapType.AREA], Structure[oppositeRoomX, oppositeRoomY, (int)MapType.AREA]] == 2)
							{
								// Las puertas tienen un número par y los suelos de las habitaciones opuetas son impares
								Structure[doorX, doorY, (int)MapType.DOORS] = doorId;
								Structure[adjacentRoomX, adjacentRoomY, (int)MapType.DOORS] = doorId++;
								Structure[doorX - (int)direction.X, doorY - (int)direction.Y, (int)MapType.DOORS] = doorId;
								Structure[oppositeRoomX, oppositeRoomY, (int)MapType.DOORS] = doorId++;
								// Guardar la vencidad
								Neighborhood[Structure[adjacentRoomX, adjacentRoomY, (int)MapType.AREA], Structure[oppositeRoomX, oppositeRoomY, (int)MapType.AREA]] = (byte)NeighborType.DOORS;
								Neighborhood[Structure[oppositeRoomX, oppositeRoomY, (int)MapType.AREA], Structure[adjacentRoomX, adjacentRoomY, (int)MapType.AREA]] = (byte)NeighborType.DOORS;
								// Guardar la posicion de las puertas
								DoorsPositions[Structure[adjacentRoomX, adjacentRoomY, (int)MapType.AREA]].Add(new System.Numerics.Vector2(doorX, doorY));
								DoorsPositions[Structure[oppositeRoomX, oppositeRoomY, (int)MapType.AREA]].Add(new System.Numerics.Vector2(doorX - (int)direction.X, doorY - (int)direction.Y));
								// Guardar la posición del spawn
								SpawnPositions[Structure[adjacentRoomX, adjacentRoomY, (int)MapType.AREA]].Add(new System.Numerics.Vector2(adjacentRoomX, adjacentRoomY));
								SpawnPositions[Structure[oppositeRoomX, oppositeRoomY, (int)MapType.AREA]].Add(new System.Numerics.Vector2(oppositeRoomX, oppositeRoomY));
								break;
							}
						}
					}
				}
				allDoorsSet = CheckDoorsConnections();
			}
		}
	}

	private bool CheckDoorsConnections()
	{
		for (int i = 1; i < Neighborhood.GetLength(0); i++)
		{
			for (int j = 1; j < i; j++)
			{
				if (Neighborhood[i, j] == (byte)NeighborType.COUNTED)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void GenerateGround(int roomId)
	{
		List<System.Numerics.Vector2> automatas;
		automatas = new(DoorsPositions[roomId]);
		bool roomCreated = false;
		do
		{
			roomCreated = (double)RoomsSurface[roomId].ground / RoomsSurface[roomId].area > MinimumGroundPerRoom && PathConnected(automatas[0], roomId);
			for (int j = 0; j < automatas.Count; j++)
			{
				if (Structure[(int)automatas[j].X, (int)automatas[j].Y, (int)MapType.GROUND] == 0)
				{
					RoomsSurface[roomId].ground += 1;
				}
				Structure[(int)automatas[j].X, (int)automatas[j].Y, (int)MapType.GROUND] = (byte)roomId;
				automatas[j] = MoveAutomata(automatas[j]);
			}
		} while (!roomCreated);
	}

	private System.Numerics.Vector2 MoveAutomata(System.Numerics.Vector2 automata)
	{
		Random random = new();
		int randomNumber = random.Next(0, 4);
		System.Numerics.Vector2 targetPosition = automata + Directions[randomNumber];
		while (Structure[(int)targetPosition.X, (int)targetPosition.Y, (int)MapType.WALLS] != 0)
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
		List<System.Numerics.Vector2> spawns = new(SpawnPositions[roomId]);
		while (queue.Count > 0)
		{
			System.Numerics.Vector2 current = queue.Dequeue();
			foreach (var direction in Directions)
			{
				System.Numerics.Vector2 neighbor = current + direction;
				if (neighbor.X >= 0 && neighbor.X < _borders.X && neighbor.Y >= 0 && neighbor.Y < _borders.Y)
				{
					if (Structure[(int)neighbor.X, (int)neighbor.Y, (int)MapType.GROUND] != 0 && !visited[(int)neighbor.X, (int)neighbor.Y])
					{
						queue.Enqueue(neighbor);
						visited[(int)neighbor.X, (int)neighbor.Y] = true;
						if (spawns.Contains(neighbor))
						{
							spawns.Remove(neighbor);
						}
					}
				}
			}
		}
		return spawns.Count == 0;
	}
}

