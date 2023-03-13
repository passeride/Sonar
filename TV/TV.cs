using Godot;
using System;

public partial class TV : Node3D
{
    [Export]
    private bool _shows_escape_route = false;

    [Export]
    private string _active_screen_material_path = "res://TV/TV_Screen_active.tres";

    [Export]
    private string _inactive_screen_material_path = "res://TV/TV_Screen.tres";

    private NavigationAgent3D _agent;

    private StandardMaterial3D _active_screen_material;
    private StandardMaterial3D _inactive_screen_material;
    private OmniLight3D _light;

    private Vector3? _evac_point = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _active_screen_material = GD.Load<StandardMaterial3D>(this._active_screen_material_path);
        _inactive_screen_material = GD.Load<StandardMaterial3D>(
            this._inactive_screen_material_path
        );
        _light = GetNode<OmniLight3D>("TV_Light");
        _agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
    }

    public Vector3? RequestEvac()
    {
        return _evac_point;
    }

    public void setShowsEscapeRoute(bool value)
    {
        _shows_escape_route = value;
        GetNode<MeshInstance3D>("TV/TV")
            .SetSurfaceOverrideMaterial(
                1,
                this._shows_escape_route
                    ? this._active_screen_material
                    : this._inactive_screen_material
            );
        _light.Visible = _shows_escape_route;

        var evacStep = EvacPlan.Instance.EvacSteps[EvacPlan.Instance.CurrentEvacStep];
        if (evacStep.StepType == EvacStepActionType.GOTO)
        {
            var goto_step = evacStep as EvacStepGoTo;
            var evacs = GetTree().GetNodesInGroup(goto_step.GroupName);
            var best_evac_point = Vector3.Zero;
            var best_evac_point_distance = float.MaxValue;
            foreach (Node3D evac in evacs)
            {
                _agent.TargetPosition = evac.GlobalPosition;
                var distance = _agent.DistanceToTarget();
                GD.Print("Distance is ", distance);
                if (distance < best_evac_point_distance)
                {
                    best_evac_point = evac.GlobalPosition;
                    best_evac_point_distance = distance;
                }
            }

            if (best_evac_point_distance < float.MaxValue)
            {
                GD.Print("Best point is ", best_evac_point);
                _evac_point = best_evac_point;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
