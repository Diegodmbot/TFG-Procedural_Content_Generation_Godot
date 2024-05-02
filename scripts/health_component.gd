extends Node2D
class_name HealthComponent

signal health_changed
signal died

@export var max_health: int = 6
@onready var current_health = max_health

func damage(damage_amount: int):
	current_health = max(current_health - damage_amount, 0)
	health_changed.emit()
	check_death()

func check_death():
	if current_health == 0:
		emit_signal("died")

