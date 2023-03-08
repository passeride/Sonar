using Godot;
using System;

public partial class Agent : CharacterBody3D
{
    private NavigationAgent3D _agent;

    [Signal]
    public delegate void GoToPositionEventHandler(Vector3 pos);

    RayCast3D _see_raycast;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _see_raycast = GetNode<RayCast3D>("SeeCast");
    }

    private void _on_area_3d_area_entered(Area3D area)
    {
        if (area.IsInGroup("Evac"))
        {
            QueueFree();
        }
    }

    public void MoveTo(Vector3 pos)
    {
        _agent.TargetPosition = pos;
    }

    public void CheckSurroundings()
    {
        var TVs = GetTree().GetNodesInGroup("TV");

        var world = GetWorld3D().DirectSpaceState;
        foreach (Node3D tv in TVs)
        {
            _see_raycast.TargetPosition = _see_raycast.GlobalPosition - tv.GlobalPosition;
            _see_raycast.ForceRaycastUpdate();

            var coll = _see_raycast.GetCollider() as Node3D;
            if (coll == null)
                continue;

            if (coll.IsInGroup("TV"))
            {
                var evac_point = (coll.GetParent<TV>()).RequestEvac();
                if (evac_point != null)
                    MoveTo((Vector3)evac_point);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.T))
        {
            CheckSurroundings();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_agent.IsNavigationFinished())
            return;
        Vector3 currentPos = GlobalPosition;
        Vector3 nextPos = _agent.GetNextPathPosition();
        Vector3 velocity = (nextPos - currentPos).Normalized() * _agent.MaxSpeed;
        Vector3 lookAtPos = nextPos;
        lookAtPos.Y = 2.0f;
        GlobalRotation = lookAtPos;
        LookAt(lookAtPos, Vector3.Up);
        var test = RotationDegrees;
        test.X = 0.0f;
        RotationDegrees = test;
        // MoveAndSlide(velocity);
        Velocity = velocity * (float)delta;
        MoveAndSlide();
    }
}
