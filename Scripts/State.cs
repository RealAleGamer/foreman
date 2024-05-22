using Godot;
using System;

namespace FM.State;

public partial class State : Node2D
{
	public virtual string StateName{ get => throw new NotImplementedException(); }

	public virtual void Enter()
	{
	} 

	public virtual void Exit()
	{
	}

	public virtual void Process(double delta)
	{
	}

	public virtual void PhysicsProcess(double delta)
	{
	}
}
