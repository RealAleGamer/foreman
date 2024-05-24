using Godot;
using System;

namespace FM.Characters;

public partial class Dwarf : Character
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;

		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (IsOnFloor() && Velocity.X != 0)
		{
			animatedSprite.Play("Walking");
			animatedSprite.FlipH = Velocity.X < 0;
		}
		else
		{
			animatedSprite.Play("Idle");
		}

		MoveAndSlide();
	}
}
