using Godot;
using System;

public partial class fire : Area3D
{

	[Export]
	private float _fire_start_size = 10.0f;
	[Export]
	private float _fire_end_size = 100.0f;
	[Export]
	private float _fire_expand_pr_seconds = 10.0f;
	private float _current_scale = 1.0f;
	// Called when the node enters the scene tree for the first time.

		SphereShape3D area_collision;
		SphereShape3D rb_collision ;
		SphereMesh fireball_mesh ;
		CylinderMesh scorth_mesh ;
		CpuParticles3D particles ;

	public override void _Ready()
	{
		_current_scale = _fire_start_size;
		area_collision = GetNode<CollisionShape3D>("CollisionShape3D").Shape as SphereShape3D;
		rb_collision = GetNode<CollisionShape3D>("RigidBody3D/CollisionShape3D").Shape as SphereShape3D;
		fireball_mesh = GetNode<MeshInstance3D>("CollisionShape3D").Mesh as SphereMesh;
		scorth_mesh = GetNode<MeshInstance3D>("FloorScortchMesh").Mesh as CylinderMesh;
		particles = GetNode<CpuParticles3D>("FireParticles");
	}

	private void set_scale(float scale)
	{
		particles.EmissionRingRadius = scale;
		area_collision.Radius = scale;
		rb_collision.Radius = scale;
		fireball_mesh.Radius = scale;
		scorth_mesh.TopRadius = scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var new_scale = _current_scale + (_fire_expand_pr_seconds * (float) delta);
		if (new_scale <= _fire_end_size)
		{
			set_scale(new_scale);
		}
	}
}
