using System.Collections.Generic;
using Godot;

public class EvacPlan
{
    private static EvacPlan instance;

    private world _world;
    public int CurrentEvacStep = 0;

    public List<EvacStep> EvacSteps =
        new()
        {
            new EvacStepIdle(),
            new EvacStepGoTo { GroupName = "TempRefuge", WaitDuration = 2.0f },
            new EvacStepWait { WaitDuration = 20.0f },
            new EvacStepGoTo { GroupName = "Evac" }
        };

    private EvacPlan() { }

    public static EvacPlan Instance
    {
        get
        {
            if (instance == null)
                instance = new EvacPlan();
            return instance;
        }
    }

    public void RegisterWorld(world world){
        _world = world;
    }

    public void StartPlan()
    {
        if (CurrentEvacStep == 0)
        {
            IncrementStep();
        }
    }

    public void IncrementStep()
    {
        if (CurrentEvacStep + 1 < EvacSteps.Count)
        {
            CurrentEvacStep += 1;
            _world.EvacStepChanged();
        }
    }

    public EvacStep GetActiveStep()
    {
        return EvacSteps[CurrentEvacStep];
    }

    public void Update(double delta) {
        var active = GetActiveStep();
        if(active is EvacStepWait){
            var wait = active as EvacStepWait;
            wait.Update(delta);
            if(wait.Expired){
                IncrementStep();
            }
        }else if(active is EvacStepGoTo){
            var wait = active as EvacStepGoTo;
            wait.Update(delta);
            if(wait.Expired){
                IncrementStep();
            }
        }
    }
}
