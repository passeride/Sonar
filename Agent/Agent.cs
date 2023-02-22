using Godot;
using System;

public partial class Agent : CharacterBody3D
{
    private NavigationAgent3D _agent;

	[Signal]
	public delegate void GoToPositionEventHandler(Vector3 pos);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");

	}

    public void MoveTo(Vector3 pos)
    {
		_agent.TargetPosition = pos;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _PhysicsProcess(double delta)
    {
        if (_agent.IsNavigationFinished()) return;
        Vector3 currentPos = GlobalPosition;
        Vector3 nextPos = _agent.GetNextPathPosition();
        Vector3 velocity =
            (nextPos - currentPos).Normalized() * _agent.MaxSpeed;
		Vector3 lookAtPos = nextPos;
		lookAtPos.Y = 2.0f;
		GlobalRotation=lookAtPos;
		LookAt(lookAtPos, Vector3.Up);
		var test = RotationDegrees;
		test.X = 0.0f;
		RotationDegrees = test;
        // MoveAndSlide(velocity);
        Velocity = velocity * (float) delta;
        MoveAndSlide();


	}

}
