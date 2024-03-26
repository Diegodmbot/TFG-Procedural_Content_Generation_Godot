extends Node

const DIRECTIONS = [Vector2.RIGHT, Vector2.UP, Vector2.LEFT, Vector2.DOWN]


func drunkard_walk(initial_positions: Array, tiles: Array):
	var tiles_limit = tiles.size()
	var step_history = []
	while tiles_limit > step_history.size():
		for i in initial_positions.size():
			var target_position = initial_positions[i] + DIRECTIONS.pick_random()
			while not tiles.has(target_position):
				target_position = initial_positions[i] + DIRECTIONS.pick_random()
			initial_positions[i] = target_position
			if not step_history.has(initial_positions[i]):
				step_history.append(initial_positions[i])
	return step_history
