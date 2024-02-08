extends Node2D
class_name Room

var id: int
var coords: Vector2
var citizens: Array
var adjacent_rooms: Array[int]
var walls = []


func _init(id: int, coords: Vector2, citizens: Array):
	self.id = id
	self.coords = coords
	self.citizens = citizens


func set_walls():
	for point in citizens:
		for i in 8:
			var rotation = (i+1)*PI/4
			var wall = point + Vector2.RIGHT.rotated(rotation).round()
			if not citizens.has(wall):
				walls.append({"id": self.id, "coords" : wall})


func get_walls():
	var walls_coords = []
	for wall in walls:
		walls_coords.append(wall["coords"])
	return walls_coords
