using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public partial class VoronoiDiagram : Node
{
	[Export] private int PointsLimit { get; set; } = 20;
	[Export] private int PistanceToPoint { get; set; } = 10;

	Vector2 borders = new(100, 100);
	readonly List<Vector2> points = [];
	int[,] map;

	public int[,] BuildVoronoiDiagram(Vector2 newBorders)
	{
		borders = newBorders;
		map = new int[(int)borders.X, (int)borders.Y];
		// Seleccionar puntos aleatorios en el mapa
		while (points.Count < PointsLimit)
		{
			Vector2 random_point = new(GD.Randi() % borders.X, GD.Randi() % borders.Y);
			if (CanBePoint(random_point))
			{
				points.Add(random_point);
			}
		}
		// Asignar un valor a cada casilla del mapa segun el punto mas cercano
		for (int i = 0; i < borders.X; i++)
		{
			for (int j = 0; j < borders.Y; j++)
			{
				Vector2 citizen = new(i, j);
				int point_id = GetNearestPointTo(citizen);
				map[i, j] = point_id;
			}
		}
		return map;
		//return new Array<Dictionary>(points);
	}

	public bool CanBePoint(Vector2 point)
	{
		// check if the point is not too close to the other points
		foreach (Vector2 current_point in points)
		{
			if (point.DistanceTo(current_point) < PistanceToPoint)
			{
				return false;
			}
		}
		// check if the point is inside the borders
		if (point.X < PistanceToPoint || point.X > borders.X - PistanceToPoint ||
			point.Y < PistanceToPoint || point.Y > borders.Y - PistanceToPoint)
		{
			return false;
		}
		return true;
	}

	public int GetNearestPointTo(Vector2 pointB)
	{
		int lowest_id = 0;
		// distance to the closest point
		float lowest_delta = borders.X * borders.Y;
		foreach (Vector2 point in points)
		{
			float delta = point.DistanceTo(pointB);
			if (delta < lowest_delta)
			{
				lowest_delta = delta;
				lowest_id = points.IndexOf(point);
			}
		}
		return lowest_id;
	}
}
