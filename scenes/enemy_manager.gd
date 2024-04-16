extends Node

const SPAWN_TILE_DISTANCE = 5
const ENEMIES_COUNT = 10

@export var enemies_type: Array[PackedScene] = []

func generate_enemies(room_tiles: Array):
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var player_tile_position: Vector2 = PositionFixer.get_tile_position_tilemap16(player.global_position)
	var avaible_positions = get_avaible_positions(player_tile_position, room_tiles)
	for i in ENEMIES_COUNT:
		var enemy_scene = enemies_type.pick_random()
		var enemy_instance = enemy_scene.instantiate() as Node2D
		call_deferred("add_child", enemy_instance)
		var enemy_position = avaible_positions.pick_random()
		enemy_instance.global_position = enemy_position
		avaible_positions.erase(enemy_position)

func get_avaible_positions(player_tile_position: Vector2, tiles: Array):
	var avaible_tiles = []
	for tile: Vector2 in tiles:
		if player_tile_position.distance_to(tile) > SPAWN_TILE_DISTANCE:
			avaible_tiles.append(PositionFixer.fix_position_to_tilemap16(tile))
	return avaible_tiles

