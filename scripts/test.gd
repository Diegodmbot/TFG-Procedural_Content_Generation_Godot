extends Node2D

func _ready():
	$DoorsManager.connect("player_exit_door", handle_door_exit)

func handle_door_exit(door_position: Vector2):
	$Player.position = door_position
