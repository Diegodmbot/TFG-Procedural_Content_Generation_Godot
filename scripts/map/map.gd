extends Node

var pause_menu_scene = preload("res://scenes/UI/pause.tscn")

const Staring_Room_Id: int = 1

@onready var map_structure = $MapStructure
@onready var player = $Player
@onready var doors_manager = $DoorsManager
@onready var enemy_manager = $EnemyManager

@onready var player_current_room: int = Staring_Room_Id
var visited_rooms: Array

func _ready():
	GameEvents.exit_door.connect(on_exit_door)
	GameEvents.room_finished.connect(on_room_finished)
	map_structure.GenerateMapStructure()
	map_structure.DrawLockedMap()
	visited_rooms.append(Staring_Room_Id)
	generate_room(Staring_Room_Id)
	move_player_to_room(Staring_Room_Id)
	generate_enemies(Staring_Room_Id)
	settle_doors()

func _unhandled_input(event):
	if event.is_action_pressed("pause"):
		add_child(pause_menu_scene.instantiate())
		get_tree().root.set_input_as_handled()

func move_player_to_room(id: int):
	var ground_structure: Array = map_structure.GetRoom(id)
	var player_starting_position: Vector2 = ground_structure.pick_random()
	player.position =  PositionFixer.fix_position_to_tilemap16(player_starting_position)


func generate_room(room_id: int):
	map_structure.GenerateGround(room_id)
	map_structure.DrawRoom(room_id)
	map_structure.DrawUnlockedRoom(room_id)

func generate_enemies(room_id: int):
	var room_tiles: Array = map_structure.GetRoom(room_id)
	enemy_manager.generate_enemies(room_tiles)

func settle_doors():
	var doors_positions: Array = map_structure.GetDoors()
	var spawns: Array = map_structure.GetSpawnsPositions()
	for i in doors_positions.size():
		var door_position = PositionFixer.fix_position_to_tilemap16(doors_positions[i])
		var door_spawn = PositionFixer.fix_position_to_tilemap16(spawns[i])
		doors_manager.create_door(i, door_position, door_spawn)

func set_current_room(room_id: int):
	player_current_room = room_id
	if not visited_rooms.has(room_id):
		generate_room(room_id)
		generate_enemies(room_id)
		visited_rooms.append(room_id)

func on_exit_door(spawn_position: Vector2):
	var room_id = map_structure.GetRoomByPosition(spawn_position.x/16, spawn_position.y/16)
	player.position = spawn_position
	call_deferred("set_current_room", room_id)
	if visited_rooms.has(room_id):
		doors_manager.open_doors()
	$Transition.play_open_circle()

func on_room_finished():
	if visited_rooms.size() == map_structure.RoomsCount:
		GameEvents.player_wins()

