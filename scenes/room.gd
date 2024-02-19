extends Node2D
class_name Room

@onready var label = %Label

var id: int
var coords: Vector2
var citizens: Array
var area_borders = []
var neighbors = []
var avaible_doors: Array[Dictionary] # {"id": int, "walls": Array[Vector2]}


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
		for i in 8:
			var rotation = (i+1)*PI/4
			var wall = point + Vector2.RIGHT.rotated(rotation).round()
			if not citizens.has(wall):
				area_borders.append({"id": self.id, "coords" : wall})


func get_area_borders():
	var area_borders_coords = []
	for wall in area_borders:
		area_borders_coords.append(wall["coords"])
	return area_borders_coords


func add_avaible_door(room_id: int, wall: Vector2):
	var room_found = false
	for room in avaible_doors:
		if room.find_key("id") == room_id:
			room_found = true
			room["walls"].append(wall)
	if room_found == false:
		var new_room = {"id": room_id, "walls": Array([wall])}
		avaible_doors.append(new_room)
