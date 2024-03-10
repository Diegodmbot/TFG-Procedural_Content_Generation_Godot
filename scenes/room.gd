extends Node2D
class_name Room

@onready var label = %Label
@onready var random_walker_component = $RandomWalkerComponent
@onready var multi_random_walker_component = $MultiRandomWalkerComponent

var id: int
var coords: Vector2
var citizens: Array
var area_borders = []
var neighbors = []
var doors: Array[Dictionary] # {"room_id": int, "coords": Vector2}
var tile_coords = []


func init_room(_id: int, _coords: Vector2, _citizens: Array):
	self.id = _id
	self.coords = _coords
	self.citizens = _citizens
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


func add_door(room_id: int, _coords: Vector2):
	for door in doors:
		if door["room_id"] == room_id:
			return
	var new_entry = {"room_id": room_id, "coords": _coords}
	doors.append(new_entry)


func generate_tiles_map():
	var avaible_tiles = subtract_array(citizens, area_borders)
	for door in doors:
		var ground_tiles = random_walker_component.drunkard_walk(door["coords"], avaible_tiles)
		tile_coords.append_array(ground_tiles)


func generate_tiles_map_multi():
	var avaible_tiles = subtract_array(citizens, area_borders)
	#var door_positions = doors.filter(func(door): return door["coords"])
	var door_positions = []
	for door in doors:
		door_positions.append(door["coords"])
	var ground_tiles = multi_random_walker_component.drunkard_walk(door_positions, avaible_tiles)
	tile_coords.append_array(ground_tiles)


func subtract_array(from: Array, subarray: Array):
	var result = []
	for value in from:
		if not subarray.has(value):
			result.append(value)
	return result

