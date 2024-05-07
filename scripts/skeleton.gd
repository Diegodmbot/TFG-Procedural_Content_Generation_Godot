extends Enemy

@onready var enemy_radar_component = $EnemyRadarComponent
@onready var velocity_component = $VelocityComponent
@onready var health_component = $HealthComponent
@onready var death_component = $DeathComponent
@onready var bone_controller = $BoneController

@onready var animation_player = $AnimationPlayer

var aggresive = false

func _ready():
	enemy_radar_component.player_detected.connect(on_player_detection)

func _physics_process(delta):
	if aggresive:
		velocity_component.decelerate()
	else:
		velocity_component.accelerate_to_player()
	velocity_component.move(self)
	if velocity_component.velocity.floor() > Vector2(0,0):
		animation_player.play("Run")
	else:
		animation_player.play("Idle")


func on_player_detection():
	aggresive = true
	bone_controller.set_bone_ability(true)


func _on_enemy_radar_component_body_exited(body):
	aggresive = false
	bone_controller.set_bone_ability(false)


