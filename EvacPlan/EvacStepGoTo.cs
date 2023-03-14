using Godot;

public class EvacStepGoTo : EvacStep
{
    public override EvacStepActionType StepType
    {
        get => EvacStepActionType.GOTO;
    }

    public string GroupName;
}
