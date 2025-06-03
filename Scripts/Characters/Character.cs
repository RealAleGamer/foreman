using System;
using System.Linq;
using Godot;

namespace FM.Characters;


public partial class Character : CharacterBody2D
{
    [Export]
    public NavGraph navGraph;

    bool delayMove = false;
    Vector2 delayedMovePos;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        Position = navGraph.FindNearestPosition(GlobalPosition);
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity = new Vector2(0, gravity * (float)delta);
            MoveAndSlide();
        }
    }

    [Flags]
    enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }

    public double Move(Vector2 newPos)
    {
        if (delayMove)
        {
            delayMove = false;
            GlobalPosition = delayedMovePos;
        }

        var direction = Direction.None;
        if (newPos.X < GlobalPosition.X) direction |= Direction.Left;
        if (newPos.X > GlobalPosition.X) direction |= Direction.Right;
        if (newPos.Y < GlobalPosition.Y) direction |= Direction.Up;
        if (newPos.Y > GlobalPosition.Y) direction |= Direction.Down;

        var sprite = GetAnimatedSprite2D();

        sprite.FlipH = (direction & Direction.Left) != Direction.None;

        bool playBackwards = false;
        string animation;
        double runtime;
        if (((direction & Direction.Up) | (direction & Direction.Down)) == Direction.None)
        {
            animation = "Walking";
            runtime = 1.0;
        }
        else if (((direction & Direction.Left) | (direction & Direction.Right)) == Direction.None)
        {
            runtime = 2.0;
            animation = "Idle";
        }
        else
        {
            animation = "Climb";
            runtime = 2.0;
            if ((direction & Direction.Down) != Direction.None)
            {
                playBackwards = true;
            }
        }

        if (playBackwards)
        {
            sprite.PlayBackwards(animation);
            delayedMovePos = newPos;
            delayMove = true;
            sprite.FlipH = !sprite.FlipH;
        }
        else
        {
            sprite.Play(animation);
            GlobalPosition = newPos;
        }

        return runtime;
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