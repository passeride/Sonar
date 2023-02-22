using Godot;
using System;

public partial class SmokeDetetctor : Node3D
{
	[Export]
	private bool _has_detected_smoke = false;

	private CpuParticles3D _sound_viz;
	public override void _Ready()
	{
		_sound_viz = GetNode<CpuParticles3D>("SoundViz");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ActivateAlarm(){
		_sound_viz.Visible = true;
	}

	private void _on_area_3d_area_entered(Area3D area){
		if(area.IsInGroup("Fire"))
		{
			_has_detected_smoke = true;
			GetNode<world>("/root/World").FireDetected();
			ActivateAlarm();
		}
		GD.Print("Entered area", area.IsInGroup("Fire"));
	}
	private void _on_area_3d_body_entered(Node3D body)
	{
		GD.Print("Entered ", body.IsInGroup("Fire"));
		// Replace with function body.
	}

}


