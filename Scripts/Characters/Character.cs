using System;
using System.Linq;
using Godot;

namespace FM.Characters;


public partial class Character : CharacterBody2D
{
    [Export]
    public NavGraph navGraph;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        Position = Position - new Vector2(Position.X % 16, 0);
    }
    public override void _PhysicsProcess(double delta)
	{
        if (!IsOnFloor())
        {
            Velocity = new Vector2(0, gravity * (float)delta);
            MoveAndSlide();
        }
	}

    public double Move(Vector2 newPos)
    {
        var left = newPos.X < GlobalPosition.X;
        var climb = newPos.Y < GlobalPosition.Y;

        GlobalPosition = newPos;
        GetAnimatedSprite2D().Play(climb ? "Climb" : "Walking");
        GetAnimatedSprite2D().FlipH = left;
        return climb ? 2.0 : 1.0;
    }

    public double Stop()
    {
        GetAnimatedSprite2D().Play("Idle");
        return 0.0;
    }

    private AnimatedSprite2D GetAnimatedSprite2D()
    {
        return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}