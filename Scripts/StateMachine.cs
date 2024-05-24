using Godot;
using System;
using System.Collections.Generic;


namespace FM.State;

[GlobalClass]
public partial class StateMachine : Node2D
{

	[Export]
	public string InitalState { get; set; }
	private State _currentState; 
	private Dictionary<string, State> _stateMap;
	private Character _character;

	public override void _Ready()
	{
		_stateMap = new Dictionary<string, State>();

		foreach (var child in GetChildren())
		{
			var state = child as State;
			if (state == null)
			{
				GD.PrintErr("Child of state machine created that is not a State");
			}

			_stateMap.Add(state.StateName.ToLower(), state);
		}


		_character = GetParent<Character>();
		SetState(InitalState);
	}

	public override void _Process(double delta)
	{
		_currentState.Process(delta, this, _character);
	}

	public void SetState(string stateName)
	{
		var lowerStatename = stateName.ToLower();

		if (_currentState != null)
		{
			_currentState.Exit();
		}

		if (!_stateMap.ContainsKey(lowerStatename))
		{
			GD.PrintErr($"Failed to find State with StateName {stateName}");	
		}

		var newState = _stateMap[lowerStatename];
		newState.Enter();
		_currentState = newState;
	}
}
