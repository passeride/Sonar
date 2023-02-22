using Godot;
using System;

public partial class world : Node3D
{
	private Camera3D _camera;
	private RayCast3D _raycast = new RayCast3D();
	private Agent _agent;


	// [Signal]
	// public delegate void PointClickPositionEventHandler(Vector3 pos);

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
		_agent = GetNode<Agent>("Agent");
		_raycast.Name = "MouseRaycast";
		_camera.AddChild(_raycast);
		_raycast.Enabled = true;
		_raycast.ClearExceptions();
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton &&
			eventMouseButton.Pressed)
		{
			 GD.Print("Right-clicked!");
             Vector3 origin =
                 _camera.ProjectRayOrigin(eventMouseButton.Position);
             Vector3 direction =
                 _camera.ProjectRayNormal(eventMouseButton.Position);
             Vector3 end = origin + direction * 1000;
			 PhysicsDirectSpaceState3D state = GetWorld3D().DirectSpaceState;
			 var query = new PhysicsRayQueryParameters3D();
			 query.From = origin;
			 query.To = end;
			 var intersection = state.IntersectRay(query);

			 if(intersection.Count > 0){

				 GD.Print("WE hit something");
				 Vector3 hit_point = ((Vector3)intersection["position"]);
                 GetTree().CallGroup("Agents", "MoveTo", hit_point);
				 // _agent.MoveTo(hit_point);
			 }
		}
	}
}
