extends CharacterBody2D

@onready var animation_player = $AnimationPlayer
@onready var velocity_component = $VelocityComponent
@onready var enemy_radar_component = $EnemyRadarComponent

var chase_player: bool = false
var counter = 0

func _ready():
	enemy_radar_component.player_detected.connect(on_player_detection)

func _physics_process(_delta):
	if chase_player == true:
		velocity_component.accelerate_to_player()
	velocity_component.move(self)
	if velocity.x > 0:
		$Sprite2D.flip_h = false
	else:
		$Sprite2D.flip_h = true

	if velocity != Vector2.ZERO:
		animation_player.play("Run")
	else:
		animation_player.play("Idle")

func on_player_detection():
	chase_player = true
