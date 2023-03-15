using Godot;
public class EvacStepGoTo : EvacStep
{
    public string GroupName;

    public override EvacStepActionType StepType => EvacStepActionType.GOTO;

    public float WaitDuration;
    public bool Expired = false;
    public float _time_passed  = 0.0f;

    public override void Update(double delta){

        _time_passed += (float)delta;

        if(_time_passed >= WaitDuration){
            Expired = true;
        }
    }
}
