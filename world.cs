using Godot;
using System;

public partial class world : Node3D
{
    [Export]
    public PackedScene FireScene;

    [Export]
    public PackedScene AgentScene;
    private Camera3D _camera;
    private RayCast3D _raycast = new RayCast3D();

    public enum ClickState
    {
        NORMAL,
        PLACE_FIRE,
        ADD_FIRE,
        REMOVE_FIRE,
        MOVE_AGENTS
    }

    private EvacPlan _evac_plan = EvacPlan.Instance;

    private ClickState _click_state = ClickState.NORMAL;
    private MainUI _main_ui;

    // [Signal]
    // public delegate void PointClickPositionEventHandler(Vector3 pos);

    public override void _Ready()
    {
        _main_ui = GetNode<MainUI>("Camera3D/MainUI");
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
        IncrementPlan();
        GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
    }

    public void IncrementPlan()
    {
        EvacPlan.Instance.IncrementStep();
        _main_ui.UpdateEvacSteps(EvacPlan.Instance.CurrentEvacStep);
    }

    public override void _Input(InputEvent @event)
    {
        if (_click_state == ClickState.PLACE_FIRE)
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
                    Node collision = ((Node)intersection["collider"]);
                    var fire_coll = collision.GetParentOrNull<fire>();
                    GD.Print(fire_coll);
                    if (fire_coll != null)
                    {
                        fire_coll.QueueFree();
                    }
                    else
                    {
                        Vector3 hit_point = ((Vector3)intersection["position"]);
                        fire fire = (fire)FireScene.Instantiate();
                        fire.GlobalPosition = hit_point;
                        AddChild(fire);
                    }
                }
            }
        }
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
                    if (eventMouseButton.ButtonIndex == MouseButton.Left)
                    {
                        Node collision = ((Node)intersection["collider"]);
                        if (collision is Agent)
                        {
                            collision.QueueFree();
                        }
                        else
                        {
                            Vector3 hit_point = ((Vector3)intersection["position"]);
                            Agent agent = (Agent)AgentScene.Instantiate();
                            agent.GlobalPosition = hit_point;
                            AddChild(agent);
                        }
                    }
                    else if (eventMouseButton.ButtonIndex == MouseButton.Right)
                    {
                        Vector3 hit_point = ((Vector3)intersection["position"]);
                        GetTree().CallGroup("Agents", "MoveTo", hit_point);
                        GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
                    }
                }
            }
        }
    }
}
