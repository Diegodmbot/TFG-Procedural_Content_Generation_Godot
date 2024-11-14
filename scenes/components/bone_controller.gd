extends Node

const base_damage = 1
const base_scope = 5

@export var bone_scene: PackedScene

@onready var base_speed_attack: float = $Timer.wait_time
@onready var timer = $Timer

var actual_attack_scope: int

func _ready():
	actual_attack_scope = base_scope * PositionFixer.tile_size

func set_bone_ability(active: bool):
	timer.paused = !active

func _on_timer_timeout():
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var bone_instance = bone_scene.instantiate()
	owner.abilities.add_child(bone_instance)
	bone_instance.hitbox_component.damage = base_damage
	bone_instance.hitbox_component.set_collision_mask_value(2, true)
	bone_instance.hitbox_component.set_collision_mask_value(1, true)
	var skeleton_position = get_parent().global_position
	bone_instance.global_position = skeleton_position
	var player_angle = skeleton_position.angle_to_point(player.global_position)
	bone_instance.target_position = skeleton_position + Vector2(actual_attack_scope,0).rotated(player_angle)
