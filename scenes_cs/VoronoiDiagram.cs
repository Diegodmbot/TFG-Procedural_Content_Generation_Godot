using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public partial class VoronoiDiagram : Node
{
	const int DISTANCE_TO_POINT = 10;
	[Export] 
	const int POINTS_LIMIT = 20;
	Rect2 Borders { get; } = new(Vector2.Zero, new Vector2(100, 100));
	Array<Dictionary> points = [];

	public struct Region {
		public int id;
		public Vector2 coords;
		public Array<Vector2> citizens;
	}

	public override void _Ready() {
		for (int i = 0; i < POINTS_LIMIT; i++) {
			Vector2 random_point = new(GD.Randi() % (int)Borders.End.X, GD.Randi() % (int)Borders.End.Y);
			if (CanBePoint(random_point)) {
				points.Add(new Dictionary {
					{ "id", i },
					{ "coords", random_point },
					{ "citizens", new Array<Vector2>() }
				});
			}
		}
	}

	public Array<Dictionary> BuildVoronoiDiagram() {
		for (int i = 0; i < (int)Borders.Size.X; i++) {
			for (int j = 0; j < (int)Borders.Size.Y; j++) {
				Vector2 citizen = new(i, j);
				int point_id = GetNearestPointTo(citizen);
				((Array<Vector2>)points[point_id]["citizens"]).Add(citizen);
			}
		}
		return points;
	}

	public bool CanBePoint(Vector2 point) {
		// check if the point is not too close to the other points
		foreach (Dictionary current_point in points) {
			if (((Vector2)current_point["coords"]).Equals(point)) {
				return false;
			}
			if (point.DistanceTo((Vector2)current_point["coords"]) < DISTANCE_TO_POINT) {
				return false;
			}
		}
		// check if the point is inside the borders
		if (point.X < DISTANCE_TO_POINT || point.X > Borders.End.X - DISTANCE_TO_POINT ||
			point.Y < DISTANCE_TO_POINT || point.Y > Borders.End.Y - DISTANCE_TO_POINT)
		{
			return false;
		}
		return true;
	}

	public int GetNearestPointTo(Vector2 pointB) {
		int lowest_id = 0;
		// distance to the closest point
		float lowest_delta = Borders.Area;
		for (int i = 0; i < points.Count; i++) {
			float delta = ((Vector2)points[i]["coords"]).DistanceTo(pointB);
			if (delta < lowest_delta) {
				lowest_delta = delta;
				lowest_id = i;
			}
		}
		return lowest_id;
	}
}
