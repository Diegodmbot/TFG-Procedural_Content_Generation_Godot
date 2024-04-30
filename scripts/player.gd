extends CharacterBody2D

@onready var health_component: HealthComponent = $HealthComponent
@onready var velocity_component = $VelocityComponent
@onready var animation_player = $Animations/AnimationPlayer
@onready var orb = $Orb

var direction: Vector2 = Vector2.ZERO
var hitted: bool = false

func _ready():
	health_component.health_changed.connect(on_health_changed)
	%HealthBar.add_max_health(health_component.current_health)

func _physics_process(_delta):
	velocity_component.accelerate_in_direction(direction)
	velocity_component.move(self)
	if velocity.x > 0:
		$Animations/Sprite2D.flip_h = false
	else:
		$Animations/Sprite2D.flip_h = true

func _process(delta):
	direction = get_movement_vector()
	if direction != Vector2.ZERO:
		animation_player.play("Walk")
	else:
		animation_player.play("Idle")
	if hitted:
		animation_player.play("Hit")
		hitted = false
		await animation_player.animation_finished

func get_movement_vector():
	var movement_vector = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	return movement_vector.normalized()

func on_health_changed():
	%HealthBar.change_health(health_component.current_health)
	hitted = true

