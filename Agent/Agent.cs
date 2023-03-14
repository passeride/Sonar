using System.Linq;
using Godot;

public partial class Agent : CharacterBody3D
{
    [Signal]
    public delegate void GoToPositionEventHandler(Vector3 pos);

    private NavigationAgent3D _agent;
    private Vector3 _initial_position;

    private RayCast3D _see_raycast;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _see_raycast = GetNode<RayCast3D>("SeeCast");
        _initial_position = GlobalPosition;
    }

    private void Reset()
    {
        GlobalPosition = _initial_position;
        _agent.TargetPosition = _initial_position;
    }

    private void _on_area_3d_area_entered(Area3D area)
    {
        if (area.IsInGroup("Evac")) QueueFree();
    }

    public void ShowPaths(bool showPaths)
    {
        _agent.DebugEnabled = showPaths;
    }

    public void MoveTo(Vector3 pos)
    {
        _agent.TargetPosition = pos;
    }

    public void CheckSurroundings()
    {
        GetNode<SpotLight3D>("SpotLight3D").Visible = true;
        var TVs = GetTree().GetNodesInGroup("TV").ToList();

        var ordered_tvs = TVs.OrderBy(node => (node as Node3D).GlobalPosition.DistanceTo(GlobalPosition));
        var found_way = false;

        foreach (Node3D tv in ordered_tvs)
        {
            _see_raycast.TargetPosition = tv.GlobalPosition - _see_raycast.GlobalPosition;
            _see_raycast.ForceRaycastUpdate();

            var coll = _see_raycast.GetCollider() as Node3D;
            if (coll == null)
                continue;

            if (coll.IsInGroup("TV"))
            {
                var evac_point = coll.GetParent<TV>().RequestEvac();
                if (evac_point != null)
                {
                    MoveTo((Vector3)evac_point);
                    found_way = true;
                    return;
                }
            }
        }

        var TR = GetTree().GetNodesInGroup("TempRefuge").ToList().FirstOrDefault() as Node3D;
        MoveTo(TR.GlobalPosition);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.T)) CheckSurroundings();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_agent.IsNavigationFinished())
            return;
        var currentPos = GlobalPosition;
        var nextPos = _agent.GetNextPathPosition();
        var velocity = (nextPos - currentPos).Normalized() * _agent.MaxSpeed;
        Vector3 lookAtPos = nextPos;
        lookAtPos.Y = 2.0f;
        GlobalRotation = lookAtPos;
        LookAt(lookAtPos, Vector3.Up);
        // var test = RotationDegrees;
        // test.X = 0.0f;
        // RotationDegrees = test;
        // MoveAndSlide(velocity);
        Velocity = velocity * (float)delta;
        MoveAndSlide();
    }
}
