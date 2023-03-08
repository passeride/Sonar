using Godot;
using System;

public partial class MainUI : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void _on_show_fire_toggled(bool button_pressed)
	{
		GD.Print("Show fire: ", button_pressed);
		GetViewport().GetCamera3D().SetCullMaskValue(2, button_pressed);
	}

	public void _on_show_people_toggled(bool button_pressed)
	{
		GD.Print("Show people: ", button_pressed);
		GetViewport().GetCamera3D().SetCullMaskValue(3, button_pressed);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
