public class EvacStepWait : EvacStep
{
    public float WaitDuration;

    public override EvacStepActionType StepType => EvacStepActionType.WAIT;
}