using Godot;
using System.Collections.Generic;

public partial class VoronoiDiagramComponentCS : Node
{
  const int DISTANCE_TO_POINT = 10;
  const int POINTS_LIMIT = 4;
  Rect2 borders = new(Vector2.Zero, new Vector2(100, 100));
  readonly Region[] points = new Region[POINTS_LIMIT];
	
	public struct Region {
		public int id;
		public Vector2 coords;
		public List<Vector2> citizens;
	}
	
	public override void _Ready() {
		for (int i = 0; i < POINTS_LIMIT; i++) {
			Vector2 random_point = new(GD.Randi() % (int)borders.End.X, GD.Randi() % (int)borders.End.Y);
			if (CanBePoint(random_point)) {
				points[i] = new Region { id = i, coords = random_point, citizens = new List<Vector2>() };
			}
		}
	}

	public Dictionary<string, object>[] VoronoiDiagram() {
		for (int i = 0; i < (int)borders.Size.X; i++) {
			for (int j = 0; j < (int)borders.Size.Y; j++) {
				Vector2 citizen = new(i, j);
				int point_id = GetNearestPointTo(citizen);
				points[point_id].citizens.Add(citizen);
			}
		}
		Dictionary<string, object>[] regions = new Dictionary<string, object>[this.points.Length];
		for (int i = 0; i < this.points.Length; i++) {
			regions[i] = new Dictionary<string, object> {
				{ "id", this.points[i].id },
				{ "coords", this.points[i].coords },
				{ "citizens", this.points[i].citizens }
			};
		}
		return regions;
	}

	public bool CanBePoint(Vector2 point) {
		foreach (Region current_point in points) {
			if (current_point.coords == point) {
				continue;
			}
			if (point.DistanceTo(current_point.coords) < DISTANCE_TO_POINT) {
				return false;
			}
		}
		if (point.X < DISTANCE_TO_POINT || point.X > borders.End.X - DISTANCE_TO_POINT ||
			point.Y < DISTANCE_TO_POINT || point.Y > borders.End.Y - DISTANCE_TO_POINT)
		{
			return false;
		}
		return true;
	}

	public int GetNearestPointTo(Vector2 pointB) {
		int lowest_id = 0;
		float lowest_delta = borders.Area;
		for (int i = 0; i < points.Length; i++) {
			float delta = points[i].coords.DistanceTo(pointB);
			if (delta < lowest_delta) {
				lowest_delta = delta;
				lowest_id = i;
			}
		}
		return lowest_id;
	}

}
