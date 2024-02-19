extends Node2D


func _ready():
	var arr: Array[Array]
	arr.append([Vector2.ZERO,Vector2.ONE])
	print(arr[0][1])
