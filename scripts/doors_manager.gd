extends Node

signal player_exit_door

var doors: Array[Door] = []

func add_door(new_door: Door):
	doors.append(new_door)
	add_child(new_door)
	new_door.connect("player_entering", on_player_entering)
	new_door.open_door()

func on_player_entering(id : int):
	var exit_id = id + 2
	for door in doors:
		if door.id == exit_id:
			emit_signal("player_exit_door", door.position)
		door.close_door()
