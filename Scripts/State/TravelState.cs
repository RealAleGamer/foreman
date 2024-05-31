using FM.Characters;
using Godot;

namespace FM.State;


public partial class TravelState : State
{
    [Export]
    public Vector2 target; 
    
    private NavGraph navGraph;

    private Vector2[] route;
    private int nextWaypointIndex;
 
	public override string StateName => "Travel";

 	public override void Enter()
	{
        if (navGraph == null)
        {
            navGraph = GetParent<StateMachine>().GetParent<Character>().navGraph;
        }

        route = navGraph.GetPath(Position, target);
        nextWaypointIndex = 0; // TODO: In some cases this should be 1 if we are already on the right side of the first point.
	} 

	public override void Exit()
	{
	}

	public override void Process(double delta, StateMachine stateMachine, Character character)
	{
	}  
}