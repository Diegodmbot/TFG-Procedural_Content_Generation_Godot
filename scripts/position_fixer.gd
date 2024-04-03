extends Node

const CELL_SIZE: int = 16

func fix_position_to_tilemap16(position: Vector2):
	return position * CELL_SIZE + Vector2(CELL_SIZE/2, CELL_SIZE/2)
