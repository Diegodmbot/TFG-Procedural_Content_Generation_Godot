extends Camera2D

@export var zoomAmplifier: float = 1
var target_position = Vector2.ZERO

func _ready():
	self.zoom *= zoomAmplifier
	make_current()

func _process(delta):
	acquire_target()
	global_position = target_position

func acquire_target():
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player != null:
		target_position = player.global_position
