extends Node2D

@onready var voronoi_diagram_component = $VoronoiDiagramComponent
@onready var tile_map = $TileMap

var rooms : Array[Room]
var adjacent_rooms


func _ready():
	var map = voronoi_diagram_component.voronoi_diagram()
	for room in map:
		var new_room = Room.new(room["id"], room["coords"], room["citizens"])
		rooms.append(new_room)
	for room in rooms:
		room.set_walls()
	draw_map()


func find_mst():
	for room in rooms:
		pass
		# mirar cada wall y buscar si en las posiciones anexas (NSEW)
		# Si la posicion entra dentro de los bordes del mapa hay un muro de otra habitación
		# guardar esas coordenadas y buscar a que habitación pertenece




func draw_map():
	for room in rooms:
		var tile_coords = Vector2(room.id,3)
		for citizen in room.citizens:
			tile_map.set_cell(0, citizen, 1, tile_coords)
		for wall in room.get_walls():
			tile_map.set_cell(1, wall, 2, Vector2(0,0))
		tile_map.set_cell(1, room.coords, 2, Vector2(0,0))
