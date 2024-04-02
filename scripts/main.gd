extends Node2D

var map_scene = preload("res://scenes/map.tscn")


func _on_c_sharp_pressed():
	var map_instance = map_scene.instantiate()
	add_child(map_instance)
