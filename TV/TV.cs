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

	private StandardMaterial3D _active_screen_material;
	private StandardMaterial3D _inactive_screen_material;
	private OmniLight3D _light;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._active_screen_material = GD.Load<StandardMaterial3D>(this._active_screen_material_path);
		this._inactive_screen_material = GD.Load<StandardMaterial3D>(this._inactive_screen_material_path);
		this._light = GetNode<OmniLight3D>("TV_Light");

	}

	public void setShowsEscapeRoute(bool value){
		this._shows_escape_route = value;

		GetNode<MeshInstance3D>("TV/TV").SetSurfaceOverrideMaterial(1, this._shows_escape_route ? this._active_screen_material : this._inactive_screen_material);
		this._light.Visible = this._shows_escape_route;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
