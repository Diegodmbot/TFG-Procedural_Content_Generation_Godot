extends Node2D


func _ready():
	var a = [1,2,3,4]
	var b = [2,4]
	print(subtract_array(a, b))

func subtract_array(from: Array, sub_array: Array):
	var result = []
	for value in from:
		if not sub_array.has(value):
			result.append(value)
	return result
