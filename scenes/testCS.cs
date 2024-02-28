using Godot;
using System;

public partial class testCS : Node
{
	int i = 1;
	int j = 2;
    public override void _Ready()
    {
        GD.Print(i +j);
		var testLabel = GetNode("testLabel") as Label;
        GD.Print(testLabel.Text);
        var a = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        GD.Print(a);
    }
}
