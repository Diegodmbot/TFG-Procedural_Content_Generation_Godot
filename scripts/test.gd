extends Node2D


func _ready():
	var a = StateEnums.HeartState.full
	a = StateEnums.HeartState.find_key(0) #empty
	print(a)
