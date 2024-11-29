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
		if ( !Warriors.Any() ) Scene.TimeScale = 0; //placeholder for losing, TODO: implement UI for this
		LocalPosition += new Vector3( RunSpeed, Input.AnalogMove.y * StrafeSpeed, 0 ) * Time.Delta;
		foreach ( var i in Warriors )
		{
			var moveDir = LocalPosition - i.LocalPosition;
			i.Agent.MoveTo( LocalPosition );
		}
	}

	public void OnTriggerEnter( GameObject other )
	{
		if ( !other.Tags.Has( "gate" ) ) return;
		
		var gate = other.GetComponent<Gate>();
		var amount = gate.GetAmount( Warriors );
		Log.Info(amount);
		while ( amount != Warriors.Count )
		{
			if ( Warriors.Count > amount )
			{
				var w = Warriors.First();
				w.DestroyGameObject();
				Warriors.Remove( w );
			}
			if ( Warriors.Count < amount )
			{
				Warriors.Add( gate.Prefab.Clone( LocalPosition ).Components.GetInChildrenOrSelf<WarriorPathing>() );
			}
		}
	}
}
