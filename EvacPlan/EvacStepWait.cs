public class EvacStepWait : EvacStep
{
    public override EvacStepActionType StepType
    {
        get => EvacStepActionType.WAIT;
    }
    public float WaitDuration;
}
