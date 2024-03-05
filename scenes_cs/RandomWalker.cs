using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RandomWalker : Node
{
  Array<Vector2> DIRECTIONS = [Vector2.Right, Vector2.Up, Vector2.Left, Vector2.Down];
	Array<Vector2> step_history = [];
	
	public Array<Vector2> DrunkardWalk(Array<Vector2> initial_positions, Array<Vector2> tiles)
	{
		int tiles_limit = 4;
		while (tiles_limit > step_history.Count)
		{
			for (int i = 0; i < initial_positions.Count; i++)
			{
				var target_position = initial_positions[i] + DIRECTIONS[(int)GD.Randi() % DIRECTIONS.Count];
				while (!tiles.Contains(target_position))
				{
					target_position = initial_positions[i] + DIRECTIONS[(int)GD.Randi() % DIRECTIONS.Count];
				}
				initial_positions[i] = target_position;
				if (!step_history.Contains(initial_positions[i]))
				{
					step_history.Append(initial_positions[i]);
        }
			}
		}
		return step_history;
	}
}
