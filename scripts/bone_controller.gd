extends Node

signal throw_bone

const base_damage = 1

@export var bone_scene: PackedScene
@onready var base_speed_attack: float = $Timer.wait_time

var position: Vector2

func _on_timer_timeout():
	# updates position
	emit_signal("throw_bone")
	# Añadir una instancia del hueso que se mueva hacia el jugador
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var bone_instance = bone_scene.instantiate()
	add_child(bone_instance)
	bone_instance.hitbox_component.damage = base_damage
	bone_instance.target_position = player.global_position
	# Si el hueso choca contra un muro desaparece
		# mandar una señal si detecta un muro, activa una animación que lo hace más pequeño y desaparece
		# misma anim que cuando no toca nada
	# Si choca contra un jugador le hace daño y desaparece
