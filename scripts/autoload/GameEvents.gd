extends Node

signal enter_door
signal exit_door
signal room_finished

func emit_enter_door(door_id: int):
	emit_signal("enter_door", door_id)

func emit_exit_door(spawn_position: Vector2):
	emit_signal("exit_door", spawn_position)

func emit_signal_room_finished():
	emit_signal("room_finished")
