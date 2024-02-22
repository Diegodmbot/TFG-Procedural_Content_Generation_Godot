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


#func get_posible_doors_position():
	#return avaible_door_positions
#
#
#func add_avaible_door_position(room_id: int, wall: Vector2):
	#var room_found = false
	#for room in avaible_door_positions:
		#if room["room_id"] == room_id:
			#room_found = true
			#room["walls"].append(wall)
	#if room_found == false:
		#var new_room = {"room_id": room_id, "walls": Array([wall])}
		#avaible_door_positions.append(new_room)


func set_door(room_id: int, coords: Vector2):
	doors.append({"room_id": room_id, "coords": coords})
