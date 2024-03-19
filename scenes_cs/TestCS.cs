using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class TestCS : Node
{
  public override void _Ready()
  {
    List<int> a = [1, 2, 3];
    GD.Print(a[1]);
    a.RemoveAt(1);
    GD.Print(a[1]);

  }
}
