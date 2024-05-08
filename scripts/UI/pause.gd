extends CanvasLayer

func _ready():
	get_tree().paused = true

func _unhandled_input(event):
	if event.is_action_pressed("pause"):
		get_tree().root.set_input_as_handled()
		queue_free()

func _exit_tree():
	get_tree().paused = false

func _on_sound_button_pressed():
	queue_free()

func _on_sound_button_2_pressed():
	get_tree().change_scene_to_file("res://scenes/main.tscn")

