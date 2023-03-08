using Godot;
using System;

public partial class world : Node3D
{
    [Export]
    public PackedScene FireScene;
    private Camera3D _camera;
    private RayCast3D _raycast = new RayCast3D();
    private Agent _agent;

    public enum ClickState
    {
        NORMAL,
        PLACE_FIRE,
        ADD_FIRE,
        REMOVE_FIRE,
        MOVE_AGENTS
    }

    private ClickState _click_state = ClickState.NORMAL;

    // [Signal]
    // public delegate void PointClickPositionEventHandler(Vector3 pos);

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _raycast.Name = "MouseRaycast";
        _camera.AddChild(_raycast);
        _raycast.Enabled = true;
        _raycast.ClearExceptions();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void SetClickState(ClickState newState)
    {
        _click_state = newState;
    }

    public void FireDetected()
    {
        // GetTree().CallGroup("Agents", "MoveTo", hit_point);
        GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
    }

    public override void _Input(InputEvent @event)
    {
        if (_click_state == ClickState.MOVE_AGENTS)
        {
            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
            {
                Vector3 origin = _camera.ProjectRayOrigin(eventMouseButton.Position);
                Vector3 direction = _camera.ProjectRayNormal(eventMouseButton.Position);
                Vector3 end = origin + direction * 1000;
                PhysicsDirectSpaceState3D state = GetWorld3D().DirectSpaceState;
                var query = new PhysicsRayQueryParameters3D();
                query.From = origin;
                query.To = end;
                var intersection = state.IntersectRay(query);

                if (intersection.Count > 0)
                {
                    Vector3 hit_point = ((Vector3)intersection["position"]);
                    GetTree().CallGroup("Agents", "MoveTo", hit_point);
                    // GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
                    // _agent.MoveTo(hit_point);
                }
            }
        }
    }
}
