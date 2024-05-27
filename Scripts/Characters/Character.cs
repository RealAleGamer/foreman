using System.Linq;
using Godot;

namespace FM.Characters;

public partial class Character : CharacterBody2D
{
    [Export]
	public float Speed = 100.0f;

    [Export]
    public NavGraph navGraph;


}