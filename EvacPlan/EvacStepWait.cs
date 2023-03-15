public class EvacStepWait : EvacStep
{
    public float WaitDuration;

    public override EvacStepActionType StepType => EvacStepActionType.WAIT;
    public bool Expired = false;
    public float _time_passed  = 0.0f;

    public override void Update(double delta){

        _time_passed += (float)delta;

        if(_time_passed >= WaitDuration){
            Expired = true;
        }
    }

}
