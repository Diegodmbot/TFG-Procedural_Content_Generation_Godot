extends Enemy

@onready var velocity_component = $VelocityComponent
@onready var enemy_radar_component = $EnemyRadarComponent
@onready var hitbox_component = $Hitbox_component
@onready var health_component = $HealthComponent
@onready var death_component = $DeathComponent
@onready var hurt_component = $HurtComponent
@onready var flinch_component = $Flinch_component

@onready var animation_player = $AnimationPlayer

var chase_player: bool = false
var counter = 0

func _ready():
	enemy_radar_component.player_detected.connect(on_player_detection)
	hurt_component.hurted.connect(on_hurted)

func _physics_process(_delta):
	if chase_player:
		velocity_component.accelerate_to_player()
	else:
		velocity_component.decelerate()
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

func on_hurted():
	$HurtAudioStream.play()
	flinch_component.flinch()

func _on_death_component_died():
	remove_child(velocity_component)
