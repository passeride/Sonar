using System.Collections.Generic;
using Godot;

public partial class StairTeleporter : NavigationLink3D
{
    private Area3D _enter;
    private Area3D _exit;

    private readonly List<Node3D> _ignore_queue_enter = new();
    private readonly List<Node3D> _ignore_queue_exit = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _enter = GetNode<Area3D>("Enter");
        _exit = GetNode<Area3D>("Exit");

        StartPosition = _enter.Position;
        EndPosition = _exit.Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_enter_body_entered_enter_node(Node3D body)
    {
        if (!_ignore_queue_exit.Contains(body) && !_ignore_queue_enter.Contains(body))
        {
            body.GlobalPosition = _exit.GlobalPosition;
            _ignore_queue_enter.Add(body);
        }
    }

    public void _on_exit_body_entered_exit(Node3D body)
    {
        if (!_ignore_queue_exit.Contains(body) && !_ignore_queue_enter.Contains(body))
        {
            body.GlobalPosition = _exit.GlobalPosition;
            _ignore_queue_exit.Add(body);
        }
    }

    public void _on_enter_body_exited_enter_node(Node3D body)
    {
        if (_ignore_queue_exit.Contains(body)) _ignore_queue_exit.Remove(body);
    }

    public void _on_exit_body_exited_exit_node(Node3D body)
    {
        if (_ignore_queue_enter.Contains(body)) _ignore_queue_enter.Remove(body);
    }
}