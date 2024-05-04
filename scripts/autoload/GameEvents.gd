extends Node

signal enter_door
signal exit_door
signal room_finished

var lose_scene = preload("res://scenes/lose_screen.tscn")

func emit_enter_door(door_id: int):
	emit_signal("enter_door", door_id)

func emit_exit_door(spawn_position: Vector2):
	emit_signal("exit_door", spawn_position)

func emit_signal_room_finished():
	emit_signal("room_finished")

func player_lose():
	get_tree().call_deferred("change_scene_to_packed", lose_scene)
