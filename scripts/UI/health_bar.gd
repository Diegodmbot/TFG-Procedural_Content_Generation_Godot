extends Control

var heart_scene = preload("res://scenes/UI/heart_visual.tscn")

@onready var h_box_container = $HBoxContainer

var current_health: int = 0

func change_health(new_health: int):
	var health_diff = current_health - new_health
	current_health = new_health
	if health_diff > 0:
		remove_health(health_diff)
	check_hp()

func add_max_health(health_amount: int):
	current_health += health_amount
	var full_hearts_count : int = floor(health_amount/2)
	var has_half_hearts : bool = health_amount%2
	for i in full_hearts_count:
		var heart_instance = heart_scene.instantiate()
		heart_instance.change_state(StateEnums.HeartState.full)
		h_box_container.add_child(heart_instance)
	if has_half_hearts:
		var heart_instance = heart_scene.instantiate()
		heart_instance.change_state(StateEnums.HeartState.half)
		h_box_container.add_child(heart_instance)

func remove_health(health_diff: int):
	var player_hearts = h_box_container.get_children() as Array[Heart]
	var i = player_hearts.size()-1
	while(health_diff > 0):
		var actual_heart_state: StateEnums.HeartState = player_hearts[i].actual_state
		if actual_heart_state == StateEnums.HeartState.empty:
			i -= 1
			continue
		player_hearts[i].change_state(actual_heart_state - 1)
		health_diff -= 1
		player_hearts[i].play_hit()

func check_hp():
	if current_health <= 2:
		var hearts = h_box_container.get_children() as Array[Heart]
		hearts[0].play_last_hp()
