using Godot;

namespace FM.State;

public partial class IdleState : State
{
    public override string StateName => "Idle";

    public override void Process(double delta)
    {
        GD.Print("Idle State: Process");        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}