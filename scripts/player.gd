extends CharacterBody2D

@onready var state_machine : AnimationNodeStateMachinePlayback = $Animations/AnimationTree["parameters/playback"]
@onready var velocity_component = $VelocityComponent

func _ready():
	pass

func _physics_process(delta):
	var movement_vector = get_movement_vector()
	velocity_component.accelerate_in_direction(movement_vector)
	velocity_component.move(self)
	if velocity.x > 0:
		$Animations/Sprite2D.flip_h = false
	else:
		$Animations/Sprite2D.flip_h = true

	move_and_slide()
	if movement_vector != Vector2.ZERO:
		state_machine.travel("Walk")
	else:
		state_machine.travel("Idle")


func get_movement_vector():
	var movement_vector = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	return movement_vector.normalized()


