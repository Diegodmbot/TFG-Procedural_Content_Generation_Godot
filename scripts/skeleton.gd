extends CharacterBody2D

@onready var enemy_radar_component = $EnemyRadarComponent
@onready var velocity_component = $VelocityComponent
@onready var animation_player = $AnimationPlayer
@onready var bone_controller = $BoneController

func _ready():
	bone_controller.throw_bone.connect(on_throw_bone)

func _physics_process(delta):
	velocity_component.move(self)

	if velocity != Vector2.ZERO:
		animation_player.play("Run")
	else:
		animation_player.play("Idle")

func on_throw_bone():
	bone_controller.position = self.position
