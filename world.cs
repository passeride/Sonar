using Godot;

public partial class world : Node3D
{
    public enum ClickState
    {
        NORMAL,
        PLACE_FIRE,
        ADD_FIRE,
        REMOVE_FIRE,
        MOVE_AGENTS
    }

    private Camera3D _camera;

    private ClickState _click_state = ClickState.NORMAL;

    private EvacPlan _evac_plan = EvacPlan.Instance;
    private MainUI _main_ui;
    private RayCast3D _raycast = new();

    [Export]
    public PackedScene AgentScene;

    [Export]
    public PackedScene FireScene;

    public override void _Ready()
    {
        _main_ui = GetNode<MainUI>("Camera3D/MainUI");
        _camera = GetNode<Camera3D>("Camera3D");
        _raycast.Name = "MouseRaycast";
        _camera.AddChild(_raycast);
        _raycast.Enabled = true;
        _raycast.ClearExceptions();
        EvacPlan.Instance.RegisterWorld(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        EvacPlan.Instance.Update(delta);
        _main_ui.UpdateEvacSteps(EvacPlan.Instance.CurrentEvacStep);
    }

    public void EvacStepChanged(){

        _main_ui.UpdateEvacSteps(EvacPlan.Instance.CurrentEvacStep);
        GetTree().CallGroup("TV", "setShowsEscapeRoute", true);
        GetTree().CallGroup("Agents", "CheckSurroundings");
    }

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
        EvacPlan.Instance.StartPlan();
        _main_ui.UpdateEvacSteps(EvacPlan.Instance.CurrentEvacStep);
    }

    public override void _Input(InputEvent @event)
    {
        if (_click_state == ClickState.PLACE_FIRE)
            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
            {
                var origin = _camera.ProjectRayOrigin(eventMouseButton.Position);
                var direction = _camera.ProjectRayNormal(eventMouseButton.Position);
                var end = origin + direction * 1000;
                var state = GetWorld3D().DirectSpaceState;
                var query = new PhysicsRayQueryParameters3D();
                query.From = origin;
                query.To = end;
                var intersection = state.IntersectRay(query);

                if (intersection.Count > 0)
                {
                    var collision = (Node)intersection["collider"];
                    var fire_coll = collision.GetParentOrNull<fire>();
                    GD.Print(fire_coll);
                    if (fire_coll != null)
                    {
                        fire_coll.QueueFree();
                    }
                    else
                    {
                        var hit_point = (Vector3)intersection["position"];
                        var fire = (fire)FireScene.Instantiate();
                        fire.GlobalPosition = hit_point;
                        AddChild(fire);
                    }
                }
            }

        if (_click_state == ClickState.MOVE_AGENTS)
            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
            {
                var origin = _camera.ProjectRayOrigin(eventMouseButton.Position);
                var direction = _camera.ProjectRayNormal(eventMouseButton.Position);
                var end = origin + direction * 3000;
                var state = GetWorld3D().DirectSpaceState;
                var query = new PhysicsRayQueryParameters3D();
                query.From = origin;
                query.To = end;
                var intersection = state.IntersectRay(query);

                if (intersection.Count > 0)
                {
                    if (eventMouseButton.ButtonIndex == MouseButton.Left)
                    {
                        var collision = (Node)intersection["collider"];
                        if (collision is Agent)
                        {
                            collision.QueueFree();
                        }
                        else
                        {
                            var hit_point = (Vector3)intersection["position"];
                            var agent = (Agent)AgentScene.Instantiate();
                            hit_point.Y += 2.0f;
                            agent.GlobalPosition = hit_point;
                            AddChild(agent);
                        }
                    }
                    else if (eventMouseButton.ButtonIndex == MouseButton.Right)
                    {
                        var hit_point = (Vector3)intersection["position"];
                        GD.Print(intersection);
                        GetTree().CallGroup("Agents", "MoveTo", hit_point);
                    }
                }
            }
    }
}
