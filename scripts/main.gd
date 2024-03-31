extends Node2D

var map_CS_scene = preload("res://scenes/MapStructure.tscn")

func clear():
	for node in $Map.get_children():
		node.queue_free()


func _on_c_sharp_pressed():
	clear()
	var map_instance = map_CS_scene.instantiate()
	$Map.add_child(map_instance)
