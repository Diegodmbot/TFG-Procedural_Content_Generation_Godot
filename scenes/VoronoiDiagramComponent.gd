extends Node

@export var points_limit = 4
@export var borders = Rect2(Vector2.ZERO, Vector2(30,30))
var points: Array = []
var map: Array[Vector2] = []

func _ready():
	while points.size() < points_limit:
		var random_point = Vector2(randi_range(0, borders.end.x-1), randi_range(0, borders.end.y-1))
		if can_be_point(random_point):
			points.append({"id": points.size(), "coords": random_point, "citizens": []})


func voronoi_diagram():
	for i in borders.size.x:
		for j in borders.size.y:
			var citizen = Vector2(i,j)
			var point_id = get_nearest_point_to(citizen)
			points[point_id]["citizens"].append(citizen)
	return points


func can_be_point(point: Vector2):
	if points.has(point):
		return false
	if point.x < 5 or point.x > borders.end.x - 5:
		return false
	if point.y < 5 or point.y > borders.end.y - 5:
		return false
	for current_point in points:
		if point.distance_to(current_point["coords"]) < 5:
			return false
	return true


func get_nearest_point_to(pointB: Vector2):
	var lowest_id = 0
	var lowest_delta = borders.get_area()
	for pointA in points:
		var delta = pointA["coords"].distance_to(pointB)
		if delta < lowest_delta:
			lowest_delta = delta
			lowest_id = pointA["id"]
	return lowest_id
