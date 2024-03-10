using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RandomWalker : Node
{
	readonly Vector2[] Directions = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];
	readonly List<Vector2> StepHistory = [];

	public Vector2[] DrunkardWalk(Vector2[] initial_positions, Vector2[] tiles)
	{
		int tiles_limit = tiles.Length / 2;
		Vector2 target_position;
		Random random = new();

		while (tiles_limit > StepHistory.Count)
		{
			for (int i = 0; i < initial_positions.Length; i++)
			{
				var randomNumber = random.Next(0, 4);
				target_position = initial_positions[i] + Directions[randomNumber];
				while (!tiles.Contains(target_position))
				{
					randomNumber = random.Next(0, 4);
					target_position = initial_positions[i] + Directions[randomNumber];
				}
				initial_positions[i] = target_position;
				if (!StepHistory.Contains(initial_positions[i]))
				{
					StepHistory.Add(initial_positions[i]);
				}
			}
		}
		Vector2[] result = [.. StepHistory];
		return result;
	}
}
