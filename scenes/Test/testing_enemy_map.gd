extends Node

@onready var map_structure = $MapStructure

func _ready():
	map_structure.GenerateMapStructure()
	for i in 3:
		var room_id = i + 1
		map_structure.GenerateGround(room_id)
		map_structure.DrawRoom(room_id)
		map_structure.DrawUnlockedRoom(room_id)
