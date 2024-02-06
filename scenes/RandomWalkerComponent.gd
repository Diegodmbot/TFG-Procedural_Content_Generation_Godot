extends Node

const DIRECTIONS = [Vector2.RIGHT, Vector2.UP, Vector2.LEFT, Vector2.DOWN]

@export var borders = Rect2(Vector2.ZERO, Vector2(50,50))
@export var tiles_limit: int = 100

var direction = Vector2.RIGHT
var current_position: Vector2
var step_history: Array[Vector2] = []
var walls_locations: Array[Vector2] = []
var available_doors_locations: Array[Vector2] = []
var doors: Array[Vector2] = []


func _ready():
	current_position = Vector2.ZERO
	assert(borders.has_point(current_position))
	step_history.append(current_position)


func drunkard_walk():
	while tiles_limit > step_history.size():
		pick_random_direction()
		current_position += direction
		if !step_history.has(current_position):
			step_history.append(current_position)
	step_history.sort()
	get_walls()
	return step_history


func get_walls():
	for tile in step_history:
		for i in 8:
			var rotation = (i+1)*PI/4
			var wall = tile + Vector2.RIGHT.rotated(rotation).round()
			if not step_history.has(wall) and not walls_locations.has(wall):
				walls_locations.append(wall)
				if is_possible_door_location(wall):
					available_doors_locations.append(wall)
	return walls_locations


func is_possible_door_location(wall: Vector2):
	pass


func get_doors():
	var door_position = walls_locations.pick_random()
	return door_position


func pick_random_direction():
	direction = DIRECTIONS.pick_random()
	while not can_move():
		direction = DIRECTIONS.pick_random()


func can_move():
	var target_position = current_position + direction
	return borders.has_point(target_position)
