extends Node2D
class_name HurtboxComponent

@export var health_component: HealthComponent

func _on_area_entered(area):
	if health_component == null:
		return
	if not area is HitboxComponent:
		return
	var hitbox_component = area as HitboxComponent
	health_component.damage(hitbox_component.damage)
