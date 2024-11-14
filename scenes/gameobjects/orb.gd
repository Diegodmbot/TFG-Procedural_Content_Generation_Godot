extends Node2D

var starting_position: Vector2

func _ready():
	starting_position = position
	position += Vector2(0, -2)
	while(true):
		var tween = create_tween()
		tween.tween_property(self, "position", starting_position + Vector2(0, -2), 1)\
		.set_ease(Tween.EASE_IN_OUT).set_trans(Tween.TRANS_BACK)
		tween.tween_property(self, "position", starting_position + Vector2(0, 1), 1)\
		.set_ease(Tween.EASE_IN_OUT).set_trans(Tween.TRANS_BACK)
		await tween.finished
