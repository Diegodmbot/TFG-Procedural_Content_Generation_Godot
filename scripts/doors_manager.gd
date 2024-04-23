extends Node

signal player_going_out

var doors: Array[Door] = []

func _ready():
	GameEvents.enter_door.connect(on_player_entering)

func add_door(new_door: Door):
	doors.append(new_door)
	add_child(new_door)
	new_door.open_door()

func on_player_entering(id : int):
	var exit_id = id + 1 if id % 2 == 0 else id - 1
	for door in doors:
		if door.id == exit_id:
			GameEvents.emit_exit_door(door.spawn_position)
		door.close_door()
