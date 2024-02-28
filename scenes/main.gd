extends Node2D


var map_scene = preload("res://scenes/map.tscn")
var map2_scene = preload("res://scenes/map2.tscn")
var voronoi_diagram_scene = preload("res://scenes/voronoi_room.tscn")

func clear():
	for node in $Rooms.get_children():
		node.queue_free()


func _on_button_pressed():
	clear()
	var map2_instance = map2_scene.instantiate()
	$Rooms.add_child(map2_instance)


func _on_button_3_pressed():
	clear()
	var map_instance = map_scene.instantiate()
	$Rooms.add_child(map_instance)
