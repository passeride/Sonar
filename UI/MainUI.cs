using Godot;

public partial class MainUI : Control
{
    private ItemList _evac_plan;
    private PanelContainer _settings_panel;
    private RichTextLabel _time_label;
    private double _timestamp_start_drill;

    public bool isDrillRunning { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _settings_panel = GetNode<PanelContainer>("Settings");
        _time_label = GetNode<RichTextLabel>("TimerLabel");
        _evac_plan = GetNode<ItemList>("EvacPlan/ItemList");
    }

    public void _on_show_fire_toggled(bool button_pressed)
    {
        GetViewport().GetCamera3D().SetCullMaskValue(2, button_pressed);
    }

    public void _on_show_people_toggled(bool button_pressed)
    {
        GetViewport().GetCamera3D().SetCullMaskValue(3, button_pressed);
    }

    public void _on_show_refuge_toggled(bool button_pressed)
    {
        GetViewport().GetCamera3D().SetCullMaskValue(4, button_pressed);
    }

    public void _on_settings_pressed()
    {
        _settings_panel.Visible = !_settings_panel.Visible;
    }

    public void _on_heli_edit_toggled(bool button_pressed)
    {
        GetNode<world>("/root/World").SetClickState(world.ClickState.NORMAL);
    }

    public void _on_fire_edit_toggled(bool button_pressed)
    {
        GetNode<world>("/root/World").SetClickState(world.ClickState.PLACE_FIRE);
    }

    public void _on_agent_edit_toggled(bool button_pressed)
    {
        GD.Print("Setting clickstate to move agetns");
        GetNode<world>("/root/World").SetClickState(world.ClickState.MOVE_AGENTS);
    }

    public void _on_show_paths_toggled(bool button_pressed)
    {
        GetTree().CallGroup("Agents", "ShowPaths", button_pressed);
    }

    public void _on_smoke_detector_slider_value_changed(float value)
    {
        GetTree().CallGroup("SmokeDetector", "SetSmokedetectorRange", value);
    }

    public void _on_start_btn_pressed()
    {
        GetTree().CallGroup("Fire", "StartFire");
        isDrillRunning = true;
        _timestamp_start_drill = Time.GetUnixTimeFromSystem();
        UpdateEvacSteps(0);
    }

    public void _on_reset_btn_pressed()
    {
        GetTree().CallGroup("Fire", "Reset");
        GetTree().CallGroup("Agents", "Reset");
        isDrillRunning = false;
    }

    public void UpdateEvacSteps(int currentStep)
    {
        for (var i = 0; i < _evac_plan.ItemCount; i++)
            if (i < currentStep)
            {
                _evac_plan.SetItemDisabled(i, true);
                _evac_plan.SetItemCustomBgColor(i, Colors.Black);
            }
            else if (i == currentStep)
            {
                _evac_plan.SetItemCustomBgColor(i, Colors.DarkGreen);
            }
    }

    public void _on_fire_growth_slider_value_changed(float value)
    {
        GetTree().CallGroup("Fire", "SetGrowthSpeed", value);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (isDrillRunning)
            _time_label.Text =
                "Timer: "
                + (Time.GetUnixTimeFromSystem() - _timestamp_start_drill).ToString(
                    "0:0.00##"
                );

        //     if (Input.IsActionJustPressed("start_sim"))
        //     {
        //         _on_start_btn_pressed();
        //     }
        //     if (Input.IsActionJustPressed("reset_sim"))
        //     {
        //         // _on_reset
        //     }
    }
}