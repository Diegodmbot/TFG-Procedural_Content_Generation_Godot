extends Node

const DIRECTIONS = [Vector2.RIGHT, Vector2.UP, Vector2.LEFT, Vector2.DOWN]

const TILES_LIMIT = 100

var avaible_tiles: Array
var direction = DIRECTIONS[0]
var current_position: Vector2

func drunkard_walk(initial_position: Vector2, tiles: Array):
	self.avaible_tiles = tiles
	current_position = initial_position
	var step_history = []
	while TILES_LIMIT > step_history.size():
		pick_random_direction()
		current_position += direction
		if !step_history.has(current_position):
			step_history.append(current_position)
	return step_history


func pick_random_direction():
	direction = DIRECTIONS.pick_random()
	while not can_move():
		direction = DIRECTIONS.pick_random()


func can_move():
	var target_position = current_position + direction
	return avaible_tiles.has(target_position)
