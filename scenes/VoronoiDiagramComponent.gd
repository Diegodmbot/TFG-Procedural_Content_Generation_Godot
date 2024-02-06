extends Node

@export var points_limit = 4
@export var borders = Rect2(Vector2.ZERO, Vector2(30,30))
var points: Array = []
var map: Array[Vector2] = []

func _ready():
	while points.size() < points_limit:
		var random_point = Vector2(randi_range(0, borders.end.x-1), randi_range(0, borders.end.y-1))
		if not points.has(random_point):
			points.append({"id": points.size(), "coords": random_point, "citizens": []})


func voronoi_diagram_euclidean():
	for i in borders.size.x:
		for j in borders.size.y:
			var citizen = Vector2(i,j)
			var point_id = get_nearest_point_to_euclidean(citizen)
			points[point_id]["citizens"].append(citizen)
	return points


func find_mst():
	pass


func voronoi_diagram_manhattan():
	for i in borders.size.x:
		for j in borders.size.y:
			var citizen = Vector2(i,j)
			var point_id = get_nearest_point_to_manhattan(citizen)
			points[point_id]["citizens"].append(citizen)
	return points


func get_nearest_point_to_euclidean(pointB: Vector2):
	var lowest_id = 0
	var lowest_delta = borders.get_area()
	for pointA in points:
		var delta = pointA["coords"].distance_to(pointB)
		if delta < lowest_delta:
			lowest_delta = delta
			lowest_id = pointA["id"]
	return lowest_id


func get_nearest_point_to_manhattan(pointB: Vector2):
	var lowest_id = 0
	var lowest_delta = borders.get_area()
	for pointA in points:
		var delta = abs(pointA["coords"].x - pointB.x) + abs(pointA["coords"].y - pointB.y)
		if delta < lowest_delta:
			lowest_delta = delta
			lowest_id = pointA["id"]
	return lowest_id

