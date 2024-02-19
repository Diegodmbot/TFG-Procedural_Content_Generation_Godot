extends Node2D


var random_walk_scene = preload("res://scenes/random_walker_room.tscn")
var voronoi_diagram_scene = preload("res://scenes/voronoi_room.tscn")
var map_scene = preload("res://scenes/map.tscn")

func clear():
	for node in $Rooms.get_children():
		node.queue_free()


func _on_button_pressed():
	clear()
	var random_walk_instance = random_walk_scene.instantiate()
	$Rooms.add_child(random_walk_instance)


func _on_button_2_pressed():
	clear()
	var voronoi_instance = voronoi_diagram_scene.instantiate()
	$Rooms.add_child(voronoi_instance)


func _on_button_3_pressed():
	clear()
	var map_instance = map_scene.instantiate()
	$Rooms.add_child(map_instance)
