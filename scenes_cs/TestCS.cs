using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class TestCS : Node
{
  public override void _Ready()
  {
    List<int> a = [1, 2, 3];
    List<int> b = new(a);
    b.Add(4);
    GD.Print(a);
  }
}
