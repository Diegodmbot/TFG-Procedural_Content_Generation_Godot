extends Node2D


func _ready():
	print(Time.get_ticks_msec())
	await get_tree().create_timer(1).timeout
	print(Time.get_ticks_msec())


func subtract_array(from: Array, sub_array: Array):
	var result = []
	for value in from:
		if not sub_array.has(value):
			result.append(value)
	return result
