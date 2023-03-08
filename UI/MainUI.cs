using Godot;
using System;

public partial class MainUI : Control
{
    PanelContainer _settings_panel;
    RichTextLabel _time_label;
    double _timestamp_start_drill = 0.0;
    bool _drill_running = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _settings_panel = GetNode<PanelContainer>("Settings");
        _time_label = GetNode<RichTextLabel>("TimerLabel");
    }

    public void _on_show_fire_toggled(bool button_pressed)
    {
        GetViewport().GetCamera3D().SetCullMaskValue(2, button_pressed);
    }

    public void _on_show_people_toggled(bool button_pressed)
    {
        GetViewport().GetCamera3D().SetCullMaskValue(3, button_pressed);
    }

    public void _on_settings_pressed()
    {
        _settings_panel.Visible = !_settings_panel.Visible;
    }

    public void _on_smoke_detector_slider_value_changed(float value)
    {
        GetTree().CallGroup("SmokeDetector", "SetSmokedetectorRange", value);
    }

    public void _on_start_btn_pressed()
    {
        GetTree().CallGroup("Fire", "StartFire");
        _drill_running = true;
        _timestamp_start_drill = Godot.Time.GetUnixTimeFromSystem();
    }

    public void _on_fire_growth_slider_value_changed(float value)
    {
        GetTree().CallGroup("Fire", "SetGrowthSpeed", value);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_drill_running)
            _time_label.Text =
                "Timer: " + (Godot.Time.GetUnixTimeFromSystem() - _timestamp_start_drill).ToString("0:0.00##");

		if (Input.IsActionJustPressed("start_sim")){
			_on_start_btn_pressed();
		}
		if (Input.IsActionJustPressed("reset_sim")){
			// _on_reset
			}
    }
}
