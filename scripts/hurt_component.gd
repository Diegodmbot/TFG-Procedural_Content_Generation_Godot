extends Area2D
class_name HurtboxComponent

enum Character_Type {None = 0, Player = 2, Enemy = 3}

@export var health_component: HealthComponent
@export var character_type: Array[Character_Type]


func _ready():
	for layer in character_type:
		set_collision_mask_value(layer, true)

func _on_area_entered(area):
	if health_component == null:
		return
	if not area is HitboxComponent:
		return
	var hitbox_component = area as HitboxComponent
	health_component.damage(hitbox_component.damage)
