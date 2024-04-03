extends Camera2D

@export var extra_zoom: float = 1
var target_position = Vector2.ZERO

func _ready():
	self.zoom *= extra_zoom
	make_current()

func _process(_delta):
	acquire_target()
	global_position = target_position

func acquire_target():
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player != null:
		target_position = player.global_position

