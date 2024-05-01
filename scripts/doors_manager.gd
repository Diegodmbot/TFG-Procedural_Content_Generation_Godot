extends Node

signal player_going_out

func _ready():
	GameEvents.enter_door.connect(on_player_entering)
	GameEvents.room_finished.connect(on_room_finished)

func add_door(new_door: Door):
	$Doors.add_child(new_door)
	new_door.close_door()

func on_player_entering(id : int):
	var exit_id = id + 1 if id % 2 == 0 else id - 1
	var doors = $Doors.get_children()
	for door in doors:
		if door.id == exit_id:
			GameEvents.emit_exit_door(door.spawn_position)
		door.close_door()

func on_room_finished():
	var doors = $Doors.get_children()
	for door in doors:
		door.open_door()
