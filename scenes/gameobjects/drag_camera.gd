extends Camera2D

var is_dragging = false
var starting_position
var mouse_starting_position
var ZOOM_ADDED = Vector2(0.3,0.3)

func _ready():
	set_process_input(true)

func _input(event):

	if event is InputEventMouseButton:
		if event.is_pressed():
			is_dragging = true
			starting_position = position
			mouse_starting_position = event.position
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				zoom += ZOOM_ADDED
			if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				var new_zoom = zoom - ZOOM_ADDED
				zoom = new_zoom if new_zoom > Vector2.ZERO else Vector2(0.1, 0.1)
		else:
			is_dragging = false
	elif event is InputEventMouseMotion and is_dragging:
		position = starting_position - (zoom + Vector2(5,5)) * (event.position - mouse_starting_position)

