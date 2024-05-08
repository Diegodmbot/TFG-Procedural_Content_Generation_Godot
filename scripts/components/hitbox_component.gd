extends Area2D
class_name HitboxComponent

@export var damage = 0

enum Character_Type {None = 0, Player = 3, Enemy = 2}

@export var character_type: Array[Character_Type]

func _ready():
	for layer in character_type:
		set_collision_layer_value(layer, true)
