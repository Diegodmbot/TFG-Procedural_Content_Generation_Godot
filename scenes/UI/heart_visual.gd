extends TextureRect
class_name Heart

var heart_full_img = preload("res://assets/HUD/heart_full.png")
var heart_half_img = preload("res://assets/HUD/heart_half.png")
var heart_empty_img = preload("res://assets/HUD/heart_empty.png")

@onready var animation_player = $AnimationPlayer

var actual_state: StateEnums.HeartState

# state indicates the distance between states
func change_state(new_state: StateEnums.HeartState):
	actual_state = new_state
	update_image()

func update_image():
	match actual_state:
		0:
			texture = heart_empty_img
		1:
			texture = heart_half_img
		2:
			texture = heart_full_img

func play_hit():
	animation_player.play("Hit")

func play_last_hp():
	animation_player.play("Last_heart")
