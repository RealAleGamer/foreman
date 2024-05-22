using Godot;

namespace FM.State;

public partial class WanderState : State
{
    public override string StateName => "Wander";

    private double _wanderTime;
    private bool _walkRight;

    public override void Process(double delta)
    {
        GD.Print("Wander State: Process");        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SetRand()
    {
        _wanderTime = GD.RandRange(2.0, 4.0);
        _walkRight = GD.Randf() > 0.5f;
    }
}