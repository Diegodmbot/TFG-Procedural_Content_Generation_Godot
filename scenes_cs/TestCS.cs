using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class TestCS : Node
{
  public override void _Ready()
  {
    GD.Print("Hello from C#!");
    int[,] arr = new int[3, 3];
    arr[0, 0] = 1;
    arr[0, 1] = 2;
    arr[0, 2] = 3;
    // VoronoiDiagram voronoiDiagram = new VoronoiDiagram();
    // var map = voronoiDiagram.BuildVoronoiDiagram(new Vector2(100, 100));
    // foreach (Dictionary point in map)
    // {
    //   GD.Print("Point: ", point["id"], " at ", ((Godot.Collections.Array)point["citizens"])[0]);
    // }
  }
}
