extends CharacterBody2D

@onready var health_component: HealthComponent = $HealthComponent
@onready var velocity_component = $VelocityComponent
@onready var animation_player = $Animations/AnimationPlayer
@onready var sword_ability_controller = $SwordAbilityController
@onready var hurt_component = $HurtComponent

var direction: Vector2 = Vector2.ZERO
var hitted: bool = false

func _ready():
	health_component.health_changed.connect(on_health_changed)
	health_component.died.connect(on_died)
	hurt_component.hurted.connect(on_hurted)
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
	if Input.is_action_pressed("melee_attack") and not hitted:
		sword_ability_controller.attack()
		velocity_component.decelerate()
	update_animation()

func set_hitted(value: bool = false):
	hitted = value

func get_movement_vector():
	var movement_vector = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	return movement_vector.normalized()

func update_animation():
	if hitted:
		animation_player.play("Hit")
	else:
		if direction != Vector2.ZERO:
			animation_player.play("Walk")
		else:
			animation_player.play("Idle")


func on_health_changed():
	%HealthBar.change_health(health_component.current_health)

func on_hurted():
	$HurtAudioStream.play()
	$HitVignette.play_hit()
	set_hitted(true)


func on_died():
	GameEvents.player_lose()

