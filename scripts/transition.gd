extends CanvasLayer

@onready var animation_player = $AnimationPlayer

func play_close_circle():
	animation_player.play("Close_circle")
	await animation_player.animation_finished
	get_tree().paused = true

func play_open_circle():
	get_tree().paused = false
	animation_player.play("Open_circle")
