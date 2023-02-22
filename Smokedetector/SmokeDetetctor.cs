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
	private void _on_area_3d_body_entered(Node3D body)
	{
		GD.Print("Entered ", body.IsInGroup("Fire"));
		// Replace with function body.
	}

}


