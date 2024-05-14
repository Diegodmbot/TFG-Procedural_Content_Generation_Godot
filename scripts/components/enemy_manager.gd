extends Node

const SPAWN_TILE_DISTANCE = 5
const ENEMIES_COUNT = 10
const LABEL_TEXT: String = "Enemies: "

@export var enemies_type: Array[PackedScene] = []
var enemies_count: int = 0

func generate_enemies(room_tiles: Array):
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var player_tile_position: Vector2 = PositionFixer.get_tile_position_tilemap16(player.global_position)
	var avaible_positions = get_avaible_positions(player_tile_position, room_tiles)
	for i in ENEMIES_COUNT:
		var enemy_scene = enemies_type.pick_random()
		var enemy_instance = enemy_scene.instantiate() as Node2D
		$Enemies.add_child(enemy_instance)
		enemy_instance.scale = Vector2(0.5, 0.5)
		var enemy_position = avaible_positions.pick_random()
		enemy_instance.global_position = enemy_position
		# Conectar señal de muerte
		enemy_instance.death_component.died.connect(on_enemy_died)
		avaible_positions.erase(enemy_position)
		enemies_count += 1
	update_counter()

func get_avaible_positions(player_tile_position: Vector2, tiles: Array):
	var avaible_tiles = []
	for tile: Vector2 in tiles:
		if player_tile_position.distance_to(tile) > SPAWN_TILE_DISTANCE:
			avaible_tiles.append(PositionFixer.fix_position_to_tilemap16(tile))
	return avaible_tiles

func on_enemy_died():
	enemies_count -= 1
	if enemies_count == 0:
		GameEvents.emit_signal_room_finished()
	update_counter()

func update_counter():
	%EnemiesCount.text = LABEL_TEXT + str(enemies_count)
