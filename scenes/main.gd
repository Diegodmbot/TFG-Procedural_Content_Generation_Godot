extends Node2D


var map_scene = preload("res://scenes/map.tscn")
var voronoi_diagram_scene = preload("res://scenes/voronoi_room.tscn")
var map_CS_scene = preload("res://scenes_cs/Map.tscn")

func clear():
	for node in $Rooms.get_children():
		node.queue_free()


func _on_gd_script_pressed():
	clear()
	var map_instance = map_scene.instantiate()
	var start_time = Time.get_ticks_msec()
	$Rooms.add_child(map_instance)
	print(Time.get_ticks_msec() - start_time)


func _on_c_sharp_pressed():
	clear()
	var map_instance = map_CS_scene.instantiate()
	var start_time = Time.get_ticks_msec()
	$Rooms.add_child(map_instance)
	print(Time.get_ticks_msec() - start_time)
