extends Node2D

var isOpen: bool = false
var id: int

func _ready():
	pass

func _process(delta):
	update_sprite()

func update_sprite():
	if isOpen:
		%Closed.visible = false
		%Opened.visible = true
	else:
		%Closed.visible = true
		%Opened.visible = false


func _on_area_2d_body_entered(body):
	if not get_tree().get_nodes_in_group("player").has(body):
		return
	isOpen = true
