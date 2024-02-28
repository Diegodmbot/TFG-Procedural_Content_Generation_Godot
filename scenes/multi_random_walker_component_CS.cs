using Godot;
using Godot.NativeInterop;
using System;
using System.Linq;
/*
extends Node

const DIRECTIONS = [Vector2.RIGHT, Vector2.UP, Vector2.LEFT, Vector2.DOWN]

func drunkard_walk(initial_positions: Array, tiles: Array):
	var tiles_limit = tiles.size() / initial_positions.size()
	var step_history = []
	while tiles_limit > step_history.size():
		for i in initial_positions.size():
			var target_position = initial_positions[i] + DIRECTIONS.pick_random()
			while not tiles.has(target_position):
				target_position = initial_positions[i] + DIRECTIONS.pick_random()
			initial_positions[i] = target_position
			if not step_history.has(initial_positions[i]):
				step_history.append(initial_positions[i])
	return step_history

*/

public partial class multi_random_walker_component_CS : Node
{
  Vector2[] DIRECTIONS = [Vector2.Right, Vector2.Up, Vector2.Left, Vector2.Down];

	public Vector2[] drunkard_walk(Vector2[] initial_positions, Vector2[] tiles)
	{
		var tiles_limit = tiles.Length / initial_positions.Length;
		var step_history = new Vector2[0];
		while (tiles_limit > step_history.Length)
		{
			for (int i = 0; i < initial_positions.Length; i++)
			{
				var target_position = initial_positions[i] + DIRECTIONS[GD.Randi() % DIRECTIONS.Length];
				while (!tiles.Contains(target_position))
				{
					target_position = initial_positions[i] + DIRECTIONS[GD.Randi() % DIRECTIONS.Length];
				}
				initial_positions[i] = target_position;
				if (!step_history.Contains(initial_positions[i]))
				{
					Array.Resize(ref step_history, step_history.Length + 1);
					step_history[step_history.Length - 1] = initial_positions[i];
				}
			}
		}
		return step_history;
	}
}
