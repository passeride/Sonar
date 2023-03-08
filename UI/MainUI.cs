using Godot;
using System;

public partial class MainUI : Control
{
	PanelContainer settings;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		settings = GetNode<PanelContainer>("Settings");
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

	public void _on_settings_pressed(){
		settings.Visible = !settings.Visible;

	}
	public void _on_smoke_detector_slider_value_changed(float value){
        GetTree().CallGroup("SmokeDetector", "SetSmokedetectorRange", value);
	}
	public void _on_fire_growth_slider_value_changed(float value){
        GetTree().CallGroup("Fire", "SetGrowthSpeed", value);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
