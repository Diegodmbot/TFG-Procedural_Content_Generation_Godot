extends Node2D


func _ready():
	var a = Vector2(0,0)
	var b = Vector2(1,0)
	var c = Vector2(1,1)
	print(a.distance_to(b))
	print(a.distance_to(c))
