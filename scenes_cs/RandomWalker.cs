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
		int tiles_limit = tiles.Count / 2;
		Vector2 target_position;

		while (tiles_limit > step_history.Count)
		{
			for (int i = 0; i < initial_positions.Count; i++)
			{
				var randomNumber = GD.RandRange(0, 3);
				target_position = initial_positions[i] + DIRECTIONS[randomNumber];
				while (!tiles.Contains(target_position))
				{
					randomNumber = GD.RandRange(0, 3);
					target_position = initial_positions[i] + DIRECTIONS[randomNumber];
				}
				initial_positions[i] = target_position;
				if (!step_history.Contains(initial_positions[i]))
				{
					step_history.Add(initial_positions[i]);
        }
			}
		}
		return step_history;
	}

    internal object DrunkardWalk(Variant variant1, Variant variant2)
    {
        throw new NotImplementedException();
    }

}
