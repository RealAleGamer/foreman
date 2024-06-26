using FM.Characters;
using Godot;

namespace FM.State;

public partial class WanderState : State
{
    public override string StateName => "Wander";

    private double _wanderTime;
    private bool _walkRight;

    public override void Process(double delta, StateMachine stateMachine, Character character)
    {
        // TODO: I know this is wrong
        character.Move(GlobalPosition + new Vector2(16, 0));

        _wanderTime -= delta;

        if (_wanderTime < 0)
        {
            stateMachine.SetState("Idle");
        }
    }

    public override void Enter()
    {
        GD.Print("Wander State: Enter");        
        SetRand();
    }

    public override void Exit()
    {
        GD.Print("Wander State: Exit");        
    }

    private void SetRand()
    {
        _wanderTime = GD.RandRange(2.0, 4.0);
        _walkRight = GD.Randf() > 0.5f;
    }
}