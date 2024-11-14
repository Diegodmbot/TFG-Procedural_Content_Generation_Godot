extends Area2D
class_name HurtboxComponent

signal hurted

enum Character_Type {None = 0, Player = 2, Enemy = 3}

@export var health_component: HealthComponent
@export var character_type: Array[Character_Type]
@export var sprite: Sprite2D

func _ready():
	for layer in character_type:
		set_collision_mask_value(layer, true)

func _on_area_entered(area):
	if health_component == null:
		return
	if not area is HitboxComponent:
		return
	if sprite != null:
		play_hurt_anim()
	var hitbox_component = area as HitboxComponent
	health_component.damage(hitbox_component.damage)
	emit_signal("hurted")

func play_hurt_anim():
	var tween = create_tween()
	tween.tween_property(sprite, "use_parent_material", false, 0.2)
	tween.tween_property(sprite, "use_parent_material", true, 0.2)

