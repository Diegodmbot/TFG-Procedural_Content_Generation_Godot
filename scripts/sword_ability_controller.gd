extends Node2D

@export var sword_scene: PackedScene

@onready var base_speed_attack: float = $Timer.wait_time
@onready var timer = $Timer

var base_damage = 5

func attack():
	if not timer.is_stopped():
		return
	var sword_instance: Node2D = sword_scene.instantiate()
	add_child(sword_instance)
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var player_position = player.global_position
	sword_instance.global_position = player_position + Vector2(0, 2)
	sword_instance.scale = Vector2(0.3, 0.3)
	var mouse_position = get_global_mouse_position()
	var sword_rotation = sword_instance.global_position.angle_to_point(mouse_position)
	sword_instance.rotation = sword_rotation
	timer.start()
