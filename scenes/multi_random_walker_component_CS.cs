using Godot;
using System;
using System.Linq;

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
