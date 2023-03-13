using Godot;
using System.Collections.Generic;

public class EvacPlan
{

    private static EvacPlan instance = null;
    private static readonly object padlock = new object();

    EvacPlan()
    {
    }

    public static EvacPlan Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new EvacPlan();
                }
                return instance;
            }
        }
    }
	public List<EvacStep> EvacSteps = new List<EvacStep>{
		new EvacStepIdle(),
		new EvacStepGoTo(){
			GroupName = "TempRefuge"
		},
		new EvacStepWait(){
			WaitDuration = ( 20.0f * 60.0f )
		},
		new EvacStepGoTo(){
			GroupName = "Evac"
		}
	};

	public int CurrentEvacStep = 0;

}
