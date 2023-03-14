public class EvacStepGoTo : EvacStep
{
    public string GroupName;

    public override EvacStepActionType StepType => EvacStepActionType.GOTO;
}