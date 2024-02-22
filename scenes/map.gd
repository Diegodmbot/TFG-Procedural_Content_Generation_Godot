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


func find_adjacent_rooms():
	var instanced_rooms = rooms.get_children()
	for i in instanced_rooms.size():
		var room = instanced_rooms[i]
		for j in range(i + 1, instanced_rooms.size()):
			var other_room = instanced_rooms[j]
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


func set_doors():
	for room in rooms.get_children() as Array[Room]:
		while room.doors.size() < room.neighbors.size():
		#for k in 3:
			var new_door = room.area_borders.pick_random()
			if map_borders.grow(-1).has_point(new_door):
				for i in 4:
					var direction = (i+1)*PI/2
					var door_entry = new_door + Vector2(1,0).rotated(direction).round()
					var door_exit = new_door + Vector2(2,0).rotated(direction + PI).round()
					if is_ground(door_entry) and is_ground(door_exit) and not room.doors.has({"room_id": room.id, "coords": new_door}):
						room.doors.append({"room_id": room.id, "coords": new_door, "entry": door_entry, "exit": door_exit})
						#room.add_door(room.id, new_door)


func is_ground(point: Vector2):
	for room in rooms.get_children() as Array[Room]:
		for cityzen in room.citizens:
			if cityzen == point:
				if room.area_borders.has(point):
					return false
				else:
					return true


func draw_map():
	for room in rooms.get_children():
		var tile_coords = Vector2(room.id,3)
		for citizen in room.citizens:
			tile_map.set_cell(0, citizen, 1, tile_coords)
		for wall in room.area_borders:
			tile_map.set_cell(1, wall, 6, Vector2(0,0))
		for wall in room.doors:
			tile_map.set_cell(2, wall["coords"], 2, Vector2(0,0))
			tile_map.set_cell(2, wall["entry"], 2, Vector2(0,0))
			tile_map.set_cell(2, wall["exit"], 2, Vector2(0,0))
