extends Node2D

@onready var voronoi_diagram_component = $VoronoiDiagramComponent
@onready var tile_map = $TileMap
@onready var rooms = $Rooms

var room_scene = preload("res://scenes/room.tscn")
var adjacent_rooms


func _ready():
	generate_rooms()
	generate_walls()
	find_adjacent_rooms()
	set_doors()
	draw_map()


func generate_rooms():
	var map = voronoi_diagram_component.voronoi_diagram()
	for room in map:
		var room_instance = room_scene.instantiate()
		rooms.add_child(room_instance)
		room_instance.set_room(room["id"], room["coords"], room["citizens"])


func generate_walls():
	for room in rooms.get_children():
		room.set_area_borders()


#refactor
func find_adjacent_rooms():
	for room in rooms.get_children():
		for other_room in rooms.get_children():
			if room != other_room:
				if is_adjacent_room(room, other_room):
					room.add_neighbor(other_room.id)


func is_adjacent_room(room: Room, other_room: Room):
	for wall in room.get_area_borders():
		if is_adjacent_point(wall, other_room):
			return true
	return false


func is_adjacent_point(point: Vector2, room: Room):
	for wall in room.get_area_borders():
		if point.distance_to(wall) <= 1:
			return true
	return false


func create_path():
	# eliminar algunos de los vecino de cada habitación
	pass


func set_doors():
	pass


func draw_map():
	for room in rooms.get_children():
		var tile_coords = Vector2(room.id,3)
		for citizen in room.citizens:
			tile_map.set_cell(0, citizen, 1, tile_coords)
		for wall in room.get_area_borders():
			tile_map.set_cell(1, wall, 2, Vector2(0,0))
