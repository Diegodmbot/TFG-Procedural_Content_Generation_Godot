using Godot;
using Godot.Collections;
using System;

public partial class TestCS : Node
{
  public override void _Ready()
  { 
    GD.Print("Voronoi");
    VoronoiDiagram voronoi = GetNode<VoronoiDiagram>("VoronoiDiagram");
    Array<Dictionary> points = voronoi.BuildVoronoiDiagram();
    foreach (Dictionary point in points)
    {
      GD.Print("Point: ", point["id"], " at ", point["coords"]);
    }
  }
}
