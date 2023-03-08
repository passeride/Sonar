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
	private bool _is_fire_running = false;

    // Called when the node enters the scene tree for the first time.

    SphereShape3D area_collision;
    SphereShape3D rb_collision;
    SphereMesh fireball_mesh;
    CylinderMesh scorth_mesh;
    CpuParticles3D particles;

    public override void _Ready()
    {
        _current_scale = _fire_start_size;
        area_collision = GetNode<CollisionShape3D>("CollisionShape3D").Shape as SphereShape3D;
        rb_collision =
            GetNode<CollisionShape3D>("RigidBody3D/CollisionShape3D").Shape as SphereShape3D;
        fireball_mesh = GetNode<MeshInstance3D>("FireballMesh").Mesh as SphereMesh;
        scorth_mesh = GetNode<MeshInstance3D>("FloorScortchMesh").Mesh as CylinderMesh;
        particles = GetNode<CpuParticles3D>("FireParticles");
    }

    public void StartFire()
    {
        Monitorable = true;
        Monitoring = true;
        set_scale(_fire_start_size);
        _current_scale = _fire_start_size;
		_is_fire_running = true;
    }

    public void Reset()
    {
        Monitorable = false;
        Monitoring = false;
        set_scale(_fire_start_size);
        _current_scale = _fire_start_size;
		_is_fire_running = false;
    }

    private void set_scale(float scale)
    {
        particles.EmissionRingRadius = scale;
        area_collision.Radius = scale;
        rb_collision.Radius = scale;
        fireball_mesh.Radius = scale;
        fireball_mesh.Height = scale * 2.0f;
        scorth_mesh.TopRadius = scale;
    }

    public void SetGrowthSpeed(float growthPrSecond)
    {
        _fire_expand_pr_seconds = growthPrSecond;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		if(!_is_fire_running)
			return;

        var new_scale = _current_scale + (_fire_expand_pr_seconds * (float)delta);
        if (new_scale <= _fire_end_size)
        {
            set_scale(new_scale);
            _current_scale = new_scale;
        }
    }
}
