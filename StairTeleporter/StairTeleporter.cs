using Godot;
using System;

public partial class StairTeleporter : NavigationLink3D
{

	private Area3D _enter;
	private Area3D _exit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enter = GetNode<Area3D>("Enter");
		_exit = GetNode<Area3D>("Exit");

		this.StartPosition = _enter.Position;
		this.EndPosition = _exit.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_enter_body_entered_enter_node(Node3D body){
		GD.Print(body);
		body.GlobalPosition = _exit.GlobalPosition;
	}

	public void _on_exit_body_entered_exit(Node3D body){

	}
	
}
