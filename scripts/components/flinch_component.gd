extends Node

@export var velocity_component: Node

func flinch():
	if owner is Enemy or owner.is_in_group("player"):
		velocity_component.accelerate_in_direction(velocity_component.velocity * -1.5)

