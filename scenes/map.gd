extends Node2D

@onready var voronoi_diagram_component = $VoronoiDiagramComponent
@onready var tile_map = $TileMap
@onready var rooms = $Rooms

var room_scene = preload("res://scenes/room.tscn")
var map_borders: Rect2

func _ready():
	map_borders = voronoi_diagram_component.borders
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


# Para reducir la complejidad del algoritmo se puede quitar la linea "for other_room in rooms.get_children():"
# del segundo para que no se compruebe la vecindad de las habitaciones dos veces. Con un solo bucle se comprobaría
# que habitaciones siguientes son vecinas de la actual
func find_adjacent_rooms():
	for room in rooms.get_children():
		for other_room in rooms.get_children():
			if room != other_room:
				for wall in room.area_borders:
					if is_adjacent_point(wall, other_room):
						room.add_neighbor(other_room.id)
						break



func is_adjacent_point(point: Vector2, room: Room):
	for wall in room.area_borders:
		if point.distance_to(wall) <= 1:
			return true
	return false


func create_path():
	# eliminar algunos de los vecino de cada habitación
	pass


func set_doors():
	for room in rooms.get_children():
		for wall in room.area_borders as Array[Vector2]:
			if map_borders.grow(-1).has_point(wall) and is_next_to_ground(room, wall):
				room.add_avaible_door_position(room.id, wall)


func is_next_to_ground(room: Room, wall: Vector2):
	for i in 4:
		var direction = (i+1)*PI/2
		var neighbor = wall + Vector2.RIGHT.rotated(direction).round()
		if room.citizens.has(neighbor) and not room.area_borders.has(neighbor) :
			return true
	return false


func draw_map():
	for room in rooms.get_children():
		var tile_coords = Vector2(room.id,3)
		for citizen in room.citizens:
			tile_map.set_cell(0, citizen, 1, tile_coords)
		for wall in room.area_borders:
			tile_map.set_cell(1, wall, 6, Vector2(0,0))
		for wall in room.get_posible_doors_position():
			for door in wall["walls"]:
				tile_map.set_cell(2, door, 2, Vector2(0,0))
		#for wall in room.area_borders:
			#tile_map.set_cell(2, wall, 2, Vector2(0,0))
