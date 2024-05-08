extends Node2D

signal died

@export var health_component: Node

func _ready():
	health_component.died.connect(on_died)

func on_died():
	if owner == null:
		return
	var tween = create_tween()
	tween.tween_property(owner, "scale", Vector2.ZERO, 0.3)
	await tween.finished
	owner.queue_free()
	emit_signal("died")
