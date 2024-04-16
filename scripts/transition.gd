extends CanvasLayer

@onready var animation_player = $AnimationPlayer

func _ready():
	GameEvents.enter_door.connect(on_enter_door)
	GameEvents.exit_door.connect(on_exit_door)

func on_enter_door():
	animation_player.play("Close_circle")
	get_tree().paused = true

func on_exit_door():
	get_tree().paused = false
	animation_player.play("Open_circle")
