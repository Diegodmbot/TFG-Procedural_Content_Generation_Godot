extends Node2D
class_name Room

@onready var label = %Label

var id: int
var coords: Vector2
var citizens: Array
var area_borders = []
var neighbors = []
#var avaible_door_positions: Array[Dictionary] # {"room_id": int, "walls": Array[Vector2]}
var doors: Array[Dictionary] # {"room_id": int, "coords": Vector2}


func set_room(id: int, coords: Vector2, citizens: Array):
	self.id = id
	self.coords = coords
	self.citizens = citizens
	label.text = str(id)
	label.position = coords * 16


func add_neighbor(room: int):
	neighbors.append(room)


func set_area_borders():
	for point in citizens:
		for i in 4:
			var rotation_radians = (i+1)*PI/2
			var wall = point + Vector2.RIGHT.rotated(rotation_radians).round()
			if not citizens.has(wall) and not area_borders.has(point):
				area_borders.append(point)


func add_door(room_id: int, coords: Vector2):
	for door in doors:
		if door["room_id"] == room_id:
			return
	var new_entry = {"room_id": room_id, "coords": coords}
	doors.append(new_entry)


func set_door(room_id: int, coords: Vector2):
	doors.append({"room_id": room_id, "coords": coords})
