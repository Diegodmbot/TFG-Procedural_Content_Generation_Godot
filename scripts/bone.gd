extends Node2D

@onready var hitbox_component = $Hitbox_component
@onready var velocity_component = $VelocityComponent

var target_position: Vector2
var t: float

func _process(delta):
	self.position = self.position.move_toward(target_position, delta * 25)

