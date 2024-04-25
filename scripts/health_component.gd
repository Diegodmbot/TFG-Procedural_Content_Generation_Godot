extends Node2D
class_name HealthComponent

signal health_changed

@export var max_health: float = 6
@onready var current_health = max_health

func damage(damage_amount: float):
	current_health = max(current_health - damage_amount, 0)
	health_changed.emit()
	print(current_health)
	check_death()

func check_death():
	if current_health == 0:
		pass
		#owner.queue_free()

