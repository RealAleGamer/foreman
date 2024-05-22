using Godot;
using System;

public partial class MainCam : Camera2D
{

	[Export]
	public float Speed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	Vector2 _moveVector;
	public override void _Process(double delta)
	{
		float fDelta = (float)delta;
		_moveVector = Vector2.Zero;
		if (Input.IsActionPressed("left"))
		{
			_moveVector.X -= fDelta;	
		}
		if (Input.IsActionPressed("right"))
		{
			_moveVector.X += fDelta;	
		}
		if (Input.IsActionPressed("down"))
		{
			_moveVector.Y += fDelta;	
		}
		if (Input.IsActionPressed("up"))
		{
			_moveVector.Y -= fDelta;	
		}

		Position += _moveVector.Normalized() * Speed;
	}
}
