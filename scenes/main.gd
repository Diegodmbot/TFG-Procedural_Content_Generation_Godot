extends Node2D

var map = preload("res://scenes/map/map.tscn")
var controls_scene = preload("res://scenes/UI/controls_screen.tscn")

func _on_play_pressed():
	get_tree().change_scene_to_packed(map)

func _on_quit_pressed():
	get_tree().quit()

func _on_controls_pressed():
	add_child(controls_scene.instantiate())
