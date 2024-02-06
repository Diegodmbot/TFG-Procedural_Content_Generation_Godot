extends Node2D

@onready var random_walker_component = $RandomWalkerComponent
@onready var tile_map_empty = $TileMapEmpty
@onready var player = $Player
@onready var camera_2d = $Camera2D

var map: Array[Vector2]


func _ready():
	randomize()
	tile_map_empty.clear()
	player.position = Vector2(8,8)
	generate_room_drunkard()


func generate_room_drunkard():
	map = random_walker_component.drunkard_walk()
	tile_map_empty.set_cells_terrain_connect(0, map, 0, 0, true)
	var walls = random_walker_component.get_walls()
	tile_map_empty.set_cells_terrain_connect(1, walls, 0, 1, true)
	var doors = random_walker_component.get_doors()
	tile_map_empty.set_cell(1,doors, 0, Vector2i(0,0))

