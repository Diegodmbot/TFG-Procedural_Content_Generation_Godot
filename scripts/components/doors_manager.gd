extends Node

signal player_going_out

@export var transition: CanvasLayer

func _ready():
	GameEvents.enter_door.connect(on_player_entering)
	GameEvents.room_finished.connect(on_room_finished)

func add_door(new_door: Door):
	$Doors.add_child(new_door)
	new_door.close_door()

func open_doors():
	var doors = $Doors.get_children()
	for door in doors:
		door.open_door()


func on_player_entering(id : int):
	if transition != null:
		transition.play_close_circle()
	var exit_id = id + 1 if id % 2 == 0 else id - 1
	var doors = $Doors.get_children()
	var exit_door_spawn: Vector2 = Vector2.ZERO
	for door in doors:
		door.close_door()
		if door.id == exit_id:
			exit_door_spawn = door.spawn_position
	GameEvents.emit_exit_door(exit_door_spawn)

func on_room_finished():
	open_doors()
