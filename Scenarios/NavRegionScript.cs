using Godot;

public partial class NavRegionScript : NavigationRegion3D
{
    private MainUI _mainUi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BakeNavigationMesh();
        _mainUi = GetNode<MainUI>("/root/World/Camera3D/MainUI");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_timer_timeout()
    {
        if (_mainUi.isDrillRunning) BakeNavigationMesh();
    }
}