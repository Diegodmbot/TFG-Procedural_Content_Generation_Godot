extends Area2D

signal player_detected

func _on_body_entered(body):
	if body.is_in_group("player"):
		emit_signal("player_detected")
