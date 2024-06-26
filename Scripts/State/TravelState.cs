using System;
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

        route = navGraph.GetPath(GlobalPosition, target);
        nextWaypointIndex = 0; // TODO: In some cases this should be 1 if we are already on the right side of the first point.
        GD.Print("Entered Travel State");
	} 

	public override void Exit()
	{
	}

    double actionTime = 0;
	public override void Process(double delta, StateMachine stateMachine, Character character)
	{
        if (actionTime > 0)
        {
            actionTime -= delta;
            return;
        }
        
        var nextWaypoint = route[nextWaypointIndex];

        actionTime = character.Move(nextWaypoint);

        nextWaypointIndex++;
        if (nextWaypointIndex >= route.Length)
        {
            stateMachine.SetState("Idle");
        }
       // QueueRedraw();
    }

    public override void _Draw()
    {
        //if (route != null && route.Length > nextWaypointIndex)
        //{
        //    DrawCircle(route[nextWaypointIndex] - GlobalPosition, 2f, new Color(1, 0, 0));
        //}
    }
}