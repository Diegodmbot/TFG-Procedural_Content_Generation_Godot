using Godot;
using Godot.Collections;
using System;


public partial class VoronoiDiagram : Node
{
	[Export] private int PointsLimit { get; set; } = 20;
	[Export] private int PistanceToPoint { get; set; } = 10;
	
	Vector2 borders = new(100, 100);
	Array<Dictionary> points = [];

	public Array<Dictionary> BuildVoronoiDiagram(Vector2 newBorders) {
		borders = newBorders;
		int id = 0;
		while (id < PointsLimit) {
			Vector2 random_point = new(GD.Randi() % borders.X, GD.Randi() % borders.Y);
			if (CanBePoint(random_point)) {
				points.Add(new Dictionary {
					{ "id", id++ },
					// El primer elemento de los vecinos es el punto mismo
					{ "citizens", new Array<Vector2> { random_point }},
				});
			}
		}
		for (int i = 0; i < borders.X; i++) {
			for (int j = 0; j < borders.Y; j++) {
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
			if (point.DistanceTo(((Array<Vector2>)current_point["citizens"])[0]) < PistanceToPoint) {
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

	public int GetNearestPointTo(Vector2 pointB) {
		int lowest_id = 0;
		// distance to the closest point
		float lowest_delta = borders.X * borders.Y;
		foreach (Dictionary point in points)
		{
			float delta = ((Array<Vector2>)point["citizens"])[0].DistanceTo(pointB);
			if (delta < lowest_delta) {
				lowest_delta = delta;
				lowest_id = (int)point["id"];
			}
		}
		return lowest_id;
	}
}
