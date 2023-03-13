    public enum EvacStepActionType
    {
        IDLE,
        GOTO,
        WAIT,
        CHECK
    }

    public abstract class EvacStep
    {
        public EvacStepActionType StepType;
    }
