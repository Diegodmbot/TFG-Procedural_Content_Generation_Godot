using Godot;
using Godot.Collections;
using System;

public partial class TestCS : Node
{
  public override void _Ready()
  {
    // This is a test
    GD.Print("Hello from C#!");
    // var voronoi = GetNode<VoronoiDiagram>("VoronoiDiagram");
    var drunkard = GetNode<RandomWalker>("RandomWalker");
    // var points = voronoi.BuildVoronoiDiagram();
    // GD.Print(points);
    Array<Vector2> initial_positions = [new Vector2(5, 5)];
    var tiles = new Array<Vector2>();
    for (int i = 0; i < 10; i++)
    {
      for (int j = 0; j < 10; j++)
      {
        tiles.Add(new Vector2(i, j));
      }
    }
    var step_history = drunkard.DrunkardWalk(initial_positions, tiles);
    GD.Print(step_history);
  }
}
