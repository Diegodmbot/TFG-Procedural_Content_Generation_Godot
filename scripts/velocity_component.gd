extends Node

@export var max_speed: int
@export var acceleration: float

var velocity = Vector2.ZERO


func accelerate_to_player():
	if owner == null:
		return
	var player = get_tree().get_first_node_in_group("player")
	if player == null:
		return

	var direction = (player.global_position - owner.global_position).normalized()
	accelerate_in_direction(direction)


func decelerate():
	accelerate_in_direction(Vector2.ZERO)


func accelerate_in_direction(direction: Vector2):
	var desired_velocity = direction * max_speed
	velocity = velocity.lerp(desired_velocity, 1 - exp(get_process_delta_time() * -acceleration))


func move(character_body: CharacterBody2D):
	character_body.velocity = velocity
	character_body.move_and_slide()
	velocity = character_body.velocity

