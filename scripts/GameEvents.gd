extends Node

signal enter_door
signal exit_door

func emit_enter_door():
	emit_signal("enter_door")

func emit_exit_door():
	emit_signal("exit_door")
