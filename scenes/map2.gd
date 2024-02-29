extends Node2D

@onready var voronoi_diagram_component = $VoronoiDiagramComponent
@onready var voronoi_diagram_component_cs = $VoronoiDiagramComponentCS
@onready var tile_map = $TileMap
@onready var rooms = $Rooms

var room_scene = preload("res://scenes/room.tscn")
var map_borders: Rect2

func _ready():
	var start_time = Time.get_ticks_msec()
	map_borders = voronoi_diagram_component_cs.borders
	generate_rooms()
	generate_borders()
	find_adjacent_rooms()
	set_doors()
	for room in rooms.get_children() as Array[Room]:
		room.generate_tiles_map_multi_2()
	draw_map()
	print(Time.get_ticks_msec() - start_time)


func generate_rooms():
	var map = voronoi_diagram_component_cs.VoronoiDiagram()
	for room in map:
		var room_instance = room_scene.instantiate()
		rooms.add_child(room_instance)
		room_instance.init_room(room["id"], room["coords"], room["citizens"])


func generate_borders():
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
						other_room.add_neighbor(room.id)
						break



func is_adjacent_point(point: Vector2, room: Room):
	for wall in room.area_borders:
		if point.distance_to(wall) <= 1:
			return true
	return false


func set_doors():
	var instanced_rooms = rooms.get_children()
	for room in instanced_rooms as Array[Room]:
		var unexplored_borders = room.area_borders.duplicate()
		while room.doors.size() < room.neighbors.size():
			var new_door = unexplored_borders.pick_random()
			unexplored_borders.erase(new_door)
			if unexplored_borders.size() < 1:
				break
			if map_borders.grow(-1).has_point(new_door):
				for i in 4:
					var direction = (i+1)*PI/2
					var door_entry = new_door + Vector2(1,0).rotated(direction).round()
					var door_exit = new_door + Vector2(2,0).rotated(direction + PI).round()
					if is_ground(door_entry) and is_ground(door_exit) and not room.doors.has({"room_id": room.id, "coords": new_door}):
						var exit_room = get_room_by_coord(door_exit)
						room.add_door(exit_room.id, new_door)
						exit_room.add_door(room.id, new_door + ((door_exit - new_door)/2))
						break


func is_ground(point: Vector2):
	for room in rooms.get_children() as Array[Room]:
		for cityzen in room.citizens:
			if cityzen == point:
				if room.area_borders.has(point):
					return false
				else:
					return true



func  get_room_by_coord(coord: Vector2):
	for room in rooms.get_children() as Array[Room]:
		if room.citizens.has(coord):
			return room


func draw_map():
	for room in rooms.get_children() as Array[Room]:
		var tile_coords = Vector2(room.id,3)
		#for citizen in room.citizens:
			#tile_map.set_cell(0, citizen, 1, tile_coords)
		for ground in room.tile_coords:
			tile_map.set_cell(0,ground, 1, tile_coords)
		for wall in room.area_borders:
			tile_map.set_cell(1, wall, 6, Vector2(0,0))
		for wall in room.doors:
			tile_map.set_cell(2, wall["coords"], 2, Vector2(0,0))
