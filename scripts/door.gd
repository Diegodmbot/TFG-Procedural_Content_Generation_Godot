extends Node2D
class_name Door

signal player_entering

@onready var animation_player: AnimationPlayer = $Animations/AnimationPlayer

var id: int = -1
var spawn_position: Vector2

func open_door():
	animation_player.play("Open")

func close_door():
	animation_player.play_backwards("Open")

func _on_area_2d_body_entered(body):
	if get_tree().get_nodes_in_group("player").has(body):
		emit_signal("player_entering", id)
