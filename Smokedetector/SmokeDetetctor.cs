using Godot;

public partial class SmokeDetetctor : Node3D
{
    [Export]
    private bool _has_detected_smoke;

    private CpuParticles3D _sound_viz;

    public override void _Ready()
    {
        _sound_viz = GetNode<CpuParticles3D>("SoundViz");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void ActivateAlarm()
    {
        _sound_viz.Visible = true;

        GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
        GetTree().CallGroup("Agents", "CheckSurroundings");
    }

    public void SetSmokedetectorRange(float range)
    {
        (GetNode<CollisionShape3D>("Area3D/SphereCollision").Shape as SphereShape3D).Radius = range;
    }

    private void _on_area_3d_area_entered(Area3D area)
    {
        if (area.IsInGroup("Fire"))
        {
            _has_detected_smoke = true;
            GetNode<world>("/root/World").FireDetected();
            ActivateAlarm();
        }
    }

    private void _on_area_3d_body_entered(Node3D body)
    {
        // Replace with function body.
    }
}