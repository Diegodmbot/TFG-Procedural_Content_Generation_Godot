extends Node2D

@onready var tile_map_empty = $TileMapEmpty
@onready var voronoi_diagram_component = $VoronoiDiagramComponent
var map: Array = []

func _ready():
	randomize()
	tile_map_empty.clear()
	generate_voronoi_diagram()


func generate_voronoi_diagram():
	map = voronoi_diagram_component.voronoi_diagram()
	for point in map:
		var tile_coords = Vector2(point["id"],3)
		tile_map_empty.set_cell(0, point["coords"], 1, tile_coords)
		for citizen in point["citizens"]:
			tile_map_empty.set_cell(0, citizen, 1, tile_coords)
