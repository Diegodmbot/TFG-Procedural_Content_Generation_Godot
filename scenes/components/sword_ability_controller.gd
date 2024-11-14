extends Node2D

@export var sword_scene: PackedScene

@onready var base_speed_attack: float = $Timer.wait_time
@onready var timer = $Timer

var base_damage = 1

func _process(delta):
	$ProgressBar.value = (timer.wait_time - timer.time_left)/timer.wait_time

func attack():
	if not timer.is_stopped():
		return
	var sword_instance: Node2D = sword_scene.instantiate()
	owner.abilities.add_child(sword_instance)
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var player_position = player.global_position
	var player_direction: Vector2 = player.player_pointing
	# ajustar la posición según la dirección del jugador
	sword_instance.global_position = player_position + Vector2(player_direction.x, player_direction.y + 5)
	sword_instance.hitbox_component.damage = base_damage
	if player_direction.x < 0:
		sword_instance.rotation = 0
		sword_instance.sword_left_sweep(0)
	else:
		var sword_rotation = Vector2.ZERO.angle_to_point(player_direction)
		sword_instance.rotation = sword_rotation
		sword_instance.sword_right_sweep(sword_rotation)
	timer.start()
