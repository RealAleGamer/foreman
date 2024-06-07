using System;
using System.Linq;
using Godot;

namespace FM.Characters;

public enum Direction
{
    Right,
    Left,
}

public partial class Character : CharacterBody2D
{
    [Export]
	public float Speed = 100.0f;

    [Export]
    public NavGraph navGraph;


    public void Walk(Direction direction)
    {
        var vel = Velocity;
        vel.X = Speed * (direction == Direction.Left ? -1 : 1);
        Velocity = vel;
        GetAnimatedSprite2D().Play("Walking");
        GetAnimatedSprite2D().FlipH = direction == Direction.Left;
    }

    public void Stop()
    {
        var vel = Velocity;
        vel.X = 0;
        Velocity = vel;
        GetAnimatedSprite2D().Play("Idle");
    }

    public void Climb(Direction direction)
    {
        GetAnimatedSprite2D().Play("Climb");
        GetAnimatedSprite2D().FlipH = direction == Direction.Left;
    }

    private AnimatedSprite2D GetAnimatedSprite2D()
    {
        return GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
}