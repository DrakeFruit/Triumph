using System.Numerics;
using Sandbox;
using Sandbox.Citizen;

public sealed class TriumphController : Component, Component.ITriggerListener
{
	[RequireComponent] public BoxCollider Collider { get; set; }
	[Property] public float StrafeSpeed { get; set; } = 250;
	[Property] public float RunSpeed { get; set; } = 200;
	[Property] public GameObject WarriorPrefab { get; set; }
	public List<WarriorPathing> Warriors { get; set; } = new();
	protected override void OnStart()
	{
		WarriorPrefab.Clone( LocalPosition );
		foreach ( var i in Scene.GetAllComponents<WarriorPathing>() )
		{
			Warriors.Add( i );
		}
	}
	protected override void OnFixedUpdate()
	{
		LocalPosition += new Vector3( RunSpeed, Input.AnalogMove.y * StrafeSpeed, 0 ) * Time.Delta;
		foreach ( var i in Warriors )
		{
			var moveDir = LocalPosition - i.LocalPosition;
			i.Agent.Velocity = moveDir * 10;
			i.Agent.MoveTo( LocalPosition );
		}
	}

	public void OnTriggerEnter( GameObject other )
	{
		if ( !other.Tags.Has( "gate" ) )
		{
			return;
		}
		
		var gate = other.Components.GetInParent<Gate>();
		Warriors = gate.SpawnPrefabs( LocalPosition, Warriors );
		if ( !Warriors.Any() ) Scene.TimeScale = 0; //placeholder for losing, TODO: implement UI for this
	}
}
