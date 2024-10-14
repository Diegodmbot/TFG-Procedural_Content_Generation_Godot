extends AudioStreamPlayer

func _ready():
	finished.connect(on_finished)

func on_finished():
	$Timer.start()

func _on_timer_timeout():
	play()
