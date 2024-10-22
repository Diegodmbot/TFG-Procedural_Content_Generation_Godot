extends Node2D

@onready var sprite = $Sprite2D
@onready var hitbox_component = $Hitbox_component

func sword_sweep(direction: int):
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2(0.8, 0.8), 0.1)
	tween.tween_property(self, "rotation", deg_to_rad(-15) + direction, 0.1)
	tween.tween_property(self, "scale", Vector2(1.3, 1.3), 0.3)
	tween.tween_property(self, "rotation", deg_to_rad(180) + direction, 0.3)
	await tween.finished
	queue_free()
