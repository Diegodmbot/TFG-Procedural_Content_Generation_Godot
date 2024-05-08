extends Node

var tile_size: int = 16

func fix_position_to_tilemap16(position: Vector2):
	return position * tile_size + Vector2(tile_size/2, tile_size/2)

func get_tile_position_tilemap16(position: Vector2):
	return (position/tile_size).floor()
