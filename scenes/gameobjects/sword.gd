extends Node2D

@onready var sprite = $Sprite2D
@onready var hitbox_component = $Hitbox_component

func sword_right_sweep(direction: float):
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2(0.8, 0.8), 0.05)
	tween.tween_property(self, "rotation", deg_to_rad(-15) + direction, 0.05)
	tween.tween_property(self, "scale", Vector2(1.2, 1.2), 0.1)
	tween.tween_property(self, "rotation", deg_to_rad(180) + direction, 0.3)
	await tween.finished
	queue_free()

func sword_left_sweep(direction: float):
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2(0.8, 0.8), 0.05)
	tween.tween_property(self, "rotation", deg_to_rad(15) + direction, 0.05)
	tween.tween_property(self, "scale", Vector2(1.2, 1.2), 0.1)
	tween.tween_property(self, "rotation", deg_to_rad(-180) + direction, 0.3)
	await tween.finished
	queue_free()
