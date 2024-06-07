using FM.Characters;
using Godot;

namespace FM.State;

public partial class IdleState : State
{
    public override string StateName => "Idle";

    private double _waitTime;

    public override void Process(double delta, StateMachine stateMachine, Character character)
    {
        character.Stop();

        _waitTime -= delta;

        if (_waitTime < 0)
        {
           stateMachine.SetState("Travel");
        }
    }

    public override void Enter()
    {
        GD.Print("Idle State: Enter");        
        SetRand();
    }

    public override void Exit()
    {
        GD.Print("Idle State: Exit");        
    }

    private void SetRand()
    {
        _waitTime = GD.RandRange(1, 5);
    }
}