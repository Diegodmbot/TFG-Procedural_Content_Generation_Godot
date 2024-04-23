extends Node2D

@onready var hitbox_component = $Hitbox_component
@onready var velocity_component = $VelocityComponent

var initial_position: Vector2
var target_position: Vector2
var t: float

func _process(delta):
	t += delta * 0.3
	self.position = initial_position.lerp(target_position, t)

