extends Node2D


func _ready():
	var a = Vector2(2,3)
	var b = Vector2(5,5)
	var distance = abs(a.x - b.x) + abs(a.y - b.y)
	var distance1 = pow((pow(b.x - a.x, 2) + pow(b.y - a.y, 2)), 1/2.0)
	var distance2 = a.distance_to(b)
	print("manhattan: " + str(distance))
	print("euclidean: " + str(distance1))
	print("method: " + str(distance2))
