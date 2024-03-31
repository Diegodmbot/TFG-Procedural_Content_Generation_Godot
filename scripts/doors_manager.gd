extends Node

signal player_exit_door
signal close_doors

func _ready():
	for child in get_children():
		if child.has_signal("player_entering"):
			child.connect("player_entering", handle_door_entry)

func handle_door_entry(id : int):
	var exit_id = id + 1 if id % 2 == 0 else id - 1
	for child: Node2D in get_children():
		if child.id == exit_id:
			emit_signal("player_exit_door", child.position)
		child.is_open = false
