extends Node

func fix_position_to_tilemap16(position: Vector2):
	return position * 16 + Vector2(8, 8)

func get_tile_position_tilemap16(position: Vector2):
	return (position/16).floor()
