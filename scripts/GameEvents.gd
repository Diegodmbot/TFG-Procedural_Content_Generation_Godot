extends Node

signal enter_door
signal exit_door

func emit_enter_door():
	enter_door.emit()

func emit_exit_door():
	exit_door.emit()
