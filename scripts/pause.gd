extends CanvasLayer

func _ready():
	get_tree().paused = true

func _unhandled_input(event):
	if event.is_action_pressed("pause"):
		close()
		get_tree().root.set_input_as_handled()

func close():
	get_tree().paused = false
	queue_free()

func _on_button_pressed():
	close()
