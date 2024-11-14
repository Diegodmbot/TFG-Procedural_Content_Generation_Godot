using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public partial class VoronoiDiagram : Node
{
	[Export] private int DistanceToPoint { get; set; } = 10;


	System.Numerics.Vector2 borders = new(100, 100);
	readonly List<System.Numerics.Vector2> points = [];
	byte[,] map;

	public byte[,] BuildVoronoiDiagram(System.Numerics.Vector2 newBorders, int pointsCount)
	{
		borders = newBorders;
		map = new byte[(int)borders.X, (int)borders.Y];
		int maxAttempts = 100;
		int attempts = 0;
		// Seleccionar puntos aleatorios en el mapa
		while (points.Count < pointsCount && attempts < maxAttempts)
		{
			System.Numerics.Vector2 random_point = new(GD.Randi() % borders.X, GD.Randi() % borders.Y);
			if (CanBePoint(random_point))
			{
				points.Add(random_point);
			}
			attempts++;
		}
		// Asignar un valor a cada casilla del mapa segun el punto mas cercano
		System.Numerics.Vector2 citizen;
		for (int i = 0; i < borders.X; i++)
		{
			for (int j = 0; j < borders.Y; j++)
			{
				citizen = new(i, j);
				byte point_id = GetNearestPointTo(citizen);
				// Se le suma uno para que no haya valores en 0, porque 0 es el valor de la casilla vacia
				map[i, j] = (byte)(point_id + 1);
			}
		}
		return map;
	}

	public bool CanBePoint(System.Numerics.Vector2 point)
	{
		// check if the point already exists
		if (points.Contains(point))
		{
			return false;
		}
		// check if the point is inside the borders
		if (point.X < DistanceToPoint || point.X > borders.X - DistanceToPoint ||
			point.Y < DistanceToPoint || point.Y > borders.Y - DistanceToPoint)
		{
			return false;
		}
		// check if the point is not too close to the other points
		foreach (System.Numerics.Vector2 current_point in points)
		{
			double distanceToPoint = Math.Pow(point.X - current_point.X, 2) + Math.Pow(point.Y - current_point.Y, 2);
			if (distanceToPoint < DistanceToPoint)
			{
				return false;
			}
		}
		return true;
	}

	public byte GetNearestPointTo(System.Numerics.Vector2 coords)
	{
		byte lowest_id = 0;
		// distance to the closest point
		double lowest_delta = borders.X * borders.Y;
		foreach (System.Numerics.Vector2 point in points)
		{
			double delta = Math.Pow(point.X - coords.X, 2) + Math.Pow(point.Y - coords.Y, 2);
			if (delta < lowest_delta)
			{
				lowest_delta = delta;
				lowest_id = (byte)points.IndexOf(point);
			}
		}
		return lowest_id;
	}
}
