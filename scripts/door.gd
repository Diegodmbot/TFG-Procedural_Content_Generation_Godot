extends Node2D

signal player_entering

@export var id: int = 0
var is_open: bool = true

func _process(delta):
	update_sprite()

func update_sprite():
	if is_open:
		%Closed.visible = false
	else:
		%Closed.visible = true

func player_going_out(door_id: int):
	if door_id == id:
		return

func _on_area_2d_body_entered(body):
	if get_tree().get_nodes_in_group("player").has(body) and is_open:
		emit_signal("player_entering", id)
