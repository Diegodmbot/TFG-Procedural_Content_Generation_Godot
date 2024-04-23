extends Node

signal throw_bone

@export var bone_scene: PackedScene
@onready var base_speed_attack: float = $Timer.wait_time

var position: Vector2
func _ready():
	print(self.position)

func _on_timer_timeout():
	# updates position
	emit_signal("throw_bone")
	# Añadir una instancia del hueso que se mueva hacia el jugador
	var player = get_tree().get_first_node_in_group("player") as Node2D
	if player == null:
		return
	var player_position = player.global_position
	var abilities_layer = get_tree().get_first_node_in_group("abilities_layer")
	# Si el hueso choca contra un muro desaparece
	# Si choca contra un jugador le hace daño y desaparece
