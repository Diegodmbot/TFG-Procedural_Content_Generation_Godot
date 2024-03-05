using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public partial class VoronoiDiagram : Node
{
	[Export] private Rect2 Borders { get; set; } = new Rect2(Vector2.Zero, new Vector2(100, 100));
	[Export] private int POINTS_LIMIT { get; set; } = 20;
	
	const int DISTANCE_TO_POINT = 10;
	Array<Dictionary> points = [];

	public Array<Dictionary> BuildVoronoiDiagram() {
		Vector2I rectEnd = (Vector2I)Borders.End;
		int counter = 0;
		while (counter < POINTS_LIMIT) {
			Vector2 random_point = new(GD.Randi() % rectEnd.X, GD.Randi() % rectEnd.Y);
			if (CanBePoint(random_point)) {
				points.Add(new Dictionary {
					{ "id", counter++ },
					{ "coords", random_point },
					{ "citizens", new Array<Vector2>() }
				});
			}
		}
		for (int i = 0; i < rectEnd.X; i++) {
			for (int j = 0; j < rectEnd.Y; j++) {
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
