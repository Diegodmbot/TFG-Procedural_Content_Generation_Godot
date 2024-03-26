using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class TestCS : Node
{
  public override void _Ready()
  {
    (int area, int ground)[] surfaces;
    surfaces = new (int, int)[10];
    GD.Print(surfaces[1]);
  }
}
