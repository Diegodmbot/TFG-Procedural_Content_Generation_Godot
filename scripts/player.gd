extends CharacterBody2D

@onready var animation_player = $Animations/AnimationPlayer
@onready var velocity_component = $VelocityComponent
@onready var orb = $Orb

func _physics_process(_delta):
	var movement_vector = get_movement_vector()
	velocity_component.accelerate_in_direction(movement_vector)
	velocity_component.move(self)
	if velocity.x > 0:
		$Animations/Sprite2D.flip_h = false
	else:
		$Animations/Sprite2D.flip_h = true

	move_and_slide()

	if movement_vector != Vector2.ZERO:
		animation_player.play("Walk")
	else:
		animation_player.play("Idle")


func get_movement_vector():
	var movement_vector = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	return movement_vector.normalized()


