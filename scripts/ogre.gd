extends CharacterBody2D

@onready var animation_player = $AnimationPlayer
@onready var velocity_component = $VelocityComponent

var counter = 0

func _physics_process(_delta):
	velocity_component.accelerate_to_player()
	velocity_component.move(self)
	if velocity.x > 0:
		$Sprite2D.flip_h = false
	else:
		$Sprite2D.flip_h = true

	move_and_slide()

	if velocity != Vector2.ZERO:
		animation_player.play("Run")
	else:
		animation_player.play("Idle")
