using System.Collections.Generic;
using Godot;

public class EvacPlan
{
    private static EvacPlan instance;

    public int CurrentEvacStep;

    public List<EvacStep> EvacSteps = new()
    {
        new EvacStepIdle(),
        new EvacStepGoTo
        {
            GroupName = "TempRefuge"
        },
        new EvacStepWait
        {
            WaitDuration = 20.0f * 60.0f
        },
        new EvacStepGoTo
        {
            GroupName = "Evac"
        }
    };

    private EvacPlan()
    {
    }

    public static EvacPlan Instance
    {
        get
        {
            if (instance == null) instance = new EvacPlan();
            return instance;
        }
    }

    public void IncrementStep()
    {
        if (CurrentEvacStep + 1 < EvacSteps.Count)
            CurrentEvacStep += 1;
    }

    public EvacStep GetActiveStep()
    {
        GD.Print("Active step is ", EvacSteps[CurrentEvacStep]);
        return EvacSteps[CurrentEvacStep];
    }
}
