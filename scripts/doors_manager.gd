extends Node

signal player_going_out

var doors: Array[Door] = []

func _ready():
	# conectar señal de GameEvents
	pass

func add_door(new_door: Door):
	doors.append(new_door)
	add_child(new_door)
	new_door.connect("player_entering", on_player_entering)
	new_door.open_door()

func on_player_entering(id : int):
	var exit_id = id + 1 if id % 2 == 0 else id - 1
	for door in doors:
		if door.id == exit_id:
			emit_signal("player_going_out", door.id, door.spawn_position)
		#door.close_door()
