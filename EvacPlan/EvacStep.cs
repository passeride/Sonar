public enum EvacStepActionType
{
    IDLE,
    GOTO,
    WAIT,
    CHECK
}

public abstract class EvacStep
{
    public abstract EvacStepActionType StepType { get; }
}