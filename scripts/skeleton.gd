extends CharacterBody2D

@onready var enemy_radar_component = $EnemyRadarComponent
@onready var velocity_component = $VelocityComponent
@onready var animation_player = $AnimationPlayer
@onready var bone_controller = $BoneController

var aggresive = false

func _ready():
	enemy_radar_component.player_detected.connect(on_player_detection)

func _physics_process(delta):
	if aggresive:
		velocity_component.accelerate_to_player()
	velocity_component.move(self)

	if velocity != Vector2.ZERO:
		animation_player.play("Run")
	else:
		animation_player.play("Idle")


func on_player_detection():
	aggresive = true
	# activar el modo disparos
