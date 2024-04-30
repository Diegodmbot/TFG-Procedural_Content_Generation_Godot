extends Node2D

@onready var hitbox_component = $Hitbox_component
@onready var animation_player = $AnimationPlayer

var target_position: Vector2
var is_moving = true

func _process(delta):
	self.position = self.position.move_toward(target_position, delta * 25)
	if self.position == target_position:
		free_bone()

func free_bone():
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector2.ZERO, 0.3)
	tween.tween_callback(queue_free)

func bone_hit():
	var tween = create_tween()
	tween.tween_property(self, "scale", scale * 2, 0.1)
	tween.tween_property(self, "scale", Vector2.ZERO, 0.2)
	tween.tween_callback(queue_free)

func _on_hitbox_component_body_entered(body):
	if body == get_tree().get_first_node_in_group("player"):
		bone_hit()
	else:
		free_bone()
