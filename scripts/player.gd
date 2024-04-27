extends CharacterBody2D

@onready var health_component: HealthComponent = $HealthComponent
@onready var velocity_component = $VelocityComponent
@onready var animation_player = $Animations/AnimationPlayer
@onready var orb = $Orb

func _ready():
	health_component.health_changed.connect(on_health_changed)
	%HealthBar.add_max_health(health_component.current_health)

func _physics_process(_delta):
	var movement_vector = get_movement_vector()
	velocity_component.accelerate_in_direction(movement_vector)
	velocity_component.move(self)
	if velocity.x > 0:
		$Animations/Sprite2D.flip_h = false
	else:
		$Animations/Sprite2D.flip_h = true

	if movement_vector != Vector2.ZERO:
		animation_player.play("Walk")
	else:
		animation_player.play("Idle")

func get_movement_vector():
	var movement_vector = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	return movement_vector.normalized()

func on_health_changed():
	%HealthBar.change_health(health_component.current_health)

